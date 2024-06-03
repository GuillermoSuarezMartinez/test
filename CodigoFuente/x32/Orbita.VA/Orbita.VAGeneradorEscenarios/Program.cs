//***********************************************************************
// Assembly         : Orbita.VAGeneradorEscenarios
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 12-12-2012
// Description      : Adaptado forma de iniciar la aplicación
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using Orbita.VAComun;
using Orbita.VAControl;

namespace Orbita.VAGeneradorEscenarios
{
    /// <summary>
    /// Programa generador de código automático
    /// </summary>
    class Program
    {
        #region Campos
        /// <summary>
        /// Indica si se han de generar las variables
        /// </summary>
        public static bool HabilitadoVariables;

        /// <summary>
        /// Indica si se han de generar el hardware
        /// </summary>
        public static bool HabilitadoHardware;
        #endregion

        #region Punto de entrada
        /// <summary>
        /// Punto de entrada de la aplicación
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            if (args.Length >= 5)
            {
                // Inicializar acceso a la base de datos
                OSistemaManager.Constructor(new OSistemaGeneradorEscenarios(args[1]));
                OSistemaManager.IniciarSistema();

                // Generación de la unidad
                OGeneradorAccesoRapido generador = new OGeneradorAccesoRapido();
                List<string> listaUsing = new List<string>() { "System", "Orbita.VAComun" };

                HabilitadoVariables = string.Equals(args[3], "Vars", StringComparison.InvariantCultureIgnoreCase);
                if (HabilitadoVariables)
                {
                    listaUsing.Add("Orbita.VAControl");
                }

                HabilitadoHardware = string.Equals(args[4], "Hard", StringComparison.InvariantCultureIgnoreCase);
                if (HabilitadoHardware)
                {
                    listaUsing.Add("Orbita.VAHardware");
                }

                generador.GenerarUnit(args[0], listaUsing.ToArray());

                // Lectura de la base de datos
                DataTable dtME = Orbita.VAControl.AppBD.GetMaquinasEstados();

                // Para cada escenario se crea su clase
                foreach (DataRow drME in dtME.Rows)
                {
                    string codME = drME["CodMaquinaEstados"].ToString();
                    string descME = drME["DescMaquinaEstados"].ToString();
                    string claseImplementadora = drME["ClaseImplementadora"].ToString();
                    string codVista = drME["CodVista"].ToString();
                    bool OK;

                    //------------ MÁQUINA DE ESTADOS ---------------//
                    CodeTypeDeclaration claseMaquinaEstados;
                    OK = GenerarMaquinaEstados(generador, claseImplementadora, descME, out claseMaquinaEstados);
                    if (!OK)
                    {
                        continue;
                    }

                    //------------ ESCENARIO ---------------//
                    string claseImplementadoraEscenario = "Escenario_" + claseImplementadora;
                    CodeTypeDeclaration claseEscenario;
                    OK = GenerarEscenario(generador, claseImplementadoraEscenario, "Escenario de " + descME, out claseEscenario);
                    if (!OK)
                    {
                        continue;
                    }

                    //------------ VARIABLES ---------------//
                    if (HabilitadoVariables)
                    {
                        GenerarVariables(generador, codVista, claseEscenario);
                    }

                    //------------ HARDWARE ---------------//
                    if (HabilitadoHardware)
                    {
                        GenerarHardware(generador, codVista, claseEscenario);
                    }

                    //------------ ESTADOS ---------------//
                    GenerarEstados(generador, codME, claseImplementadoraEscenario);

                    //------------ TRANSICIONES ---------------//
                    CodeTypeDeclaration ultimaTransicion;
                    GenerarTransiciones(generador, codME, claseImplementadoraEscenario, out ultimaTransicion);

                    //------------ REGIÓN ---------------//
                    if (ultimaTransicion != null)
                    {
                        generador.RegionStart(descME, claseMaquinaEstados);
                        generador.RegionEnd(ultimaTransicion);
                    }
                }

                // Guardar el fichero
                generador.GuardarCodigo(args[2]);
            }
        }

        #endregion

        #region Métodos estáticos
        /// <summary>
        /// Generación de la clase parcial de la máquina de estados
        /// </summary>
        /// <param name="generador"></param>
        /// <param name="descripcionMaquinaEstados"></param>
        /// <param name="claseImplementadora"></param>
        /// <param name="claseMaquinaEstados"></param>
        /// <returns></returns>
        private static bool GenerarMaquinaEstados(OGeneradorAccesoRapido generador, string claseImplementadora, string descripcionMaquinaEstados, out CodeTypeDeclaration claseMaquinaEstados)
        {
            // Generación de la clase parcial de la máquina de estados
            string clasePadre = typeof(OMaquinaEstadosBase).Name; //"OMaquinaEstadosBase"
            claseMaquinaEstados = generador.GenerarClase(claseImplementadora, clasePadre, descripcionMaquinaEstados, MemberAttributes.Public, true);
            if (claseMaquinaEstados == null)
            {
                return false;
            }

            // Generadora de constructor con la instrucción new variable
            generador.GenerarConstructor(claseMaquinaEstados, "Constructor de la clase", MemberAttributes.Public,
                new CodeParameterDeclarationExpression[] { new CodeParameterDeclarationExpression(typeof(string), "codigo") },
                new CodeExpression[] { new CodeVariableReferenceExpression("codigo") },
                new CodeStatement[] { });

            // Generadora del método de creación del escenario
            CodeStatement creacionObjetoEscenario = generador.New("_Escenario", "Escenario_" + claseImplementadora, "CodVista");
            generador.GenerarMetodo("CrearEscenario", claseMaquinaEstados, "Constructor de la clase", MemberAttributes.Public | MemberAttributes.Override,
                new CodeParameterDeclarationExpression[] { },
                new CodeStatement[] { creacionObjetoEscenario });

            // Se genera la propiedad Escenarios
            generador.GenerarPropiedadReadOnly(ref claseMaquinaEstados, "Escenario", "Escenario_" + claseImplementadora, "Escenario de la máquina de estados", MemberAttributes.Public, "Escenario_" + claseImplementadora, false);

            generador.Region(descripcionMaquinaEstados, claseMaquinaEstados);

            return true;
        }

        /// <summary>
        /// Generación de la clase parcial del escenario
        /// </summary>
        /// <param name="generador"></param>
        /// <param name="descripcionMaquinaEstados"></param>
        /// <param name="claseImplementadora"></param>
        /// <param name="claseEscenario"></param>
        /// <returns></returns>
        private static bool GenerarEscenario(OGeneradorAccesoRapido generador, string claseImplementadora, string descripcionEscenario, out CodeTypeDeclaration claseEscenario)
        {
            // Generación de la clase escenario asociada a la máquina de estados
            string clasePadre = typeof(OEscenario).Name; //"OEscenario"
            claseEscenario = generador.GenerarClase(claseImplementadora, clasePadre, descripcionEscenario, MemberAttributes.Public, true);
            if (claseEscenario == null)
            {
                return false;
            }

            // Generadora de constructor con la instrucción new variable y hardware
            List<CodeStatement> listaInstrucciones = new List<CodeStatement>();
            
            CodeStatement asignacionCodigoEscenario = generador.Asignacion("Codigo", "codigo");
            listaInstrucciones.Add(asignacionCodigoEscenario);

            if (HabilitadoVariables)
            {
                CodeStatement creacionObjetoVariables = generador.New("_Variables", "CVariables", "Codigo");
                listaInstrucciones.Add(creacionObjetoVariables);
            }
            if (HabilitadoHardware)
            {
                CodeStatement creacionObjetoHardware = generador.New("_Hardware", "CHardware", "Codigo");
                listaInstrucciones.Add(creacionObjetoHardware);
            }
            generador.GenerarConstructor(claseEscenario, "Constructor de la clase", MemberAttributes.Public,
                new CodeParameterDeclarationExpression[] { new CodeParameterDeclarationExpression(typeof(string), "codigo") },
                new CodeExpression[] { new CodeVariableReferenceExpression("codigo") },
                listaInstrucciones.ToArray());

            // Región
            generador.Region(descripcionEscenario, claseEscenario);

            return true;
        }

        /// <summary>
        /// Generación de la clase interior al escenario que implementa las variables
        /// </summary>
        /// <param name="generador"></param>
        /// <param name="codVista"></param>
        /// <param name="claseEscenario"></param>
        private static void GenerarVariables(OGeneradorAccesoRapido generador, string codVista, CodeTypeDeclaration claseEscenario)
        {
            // Se genera la propiedad Variables
            generador.GenerarPropiedadReadOnly(ref claseEscenario, "Variables", string.Empty, "Variables del escenario", MemberAttributes.Public, "CVariables", true);

            // Generador de la clase interna de vistas de variables
            CodeTypeDeclaration claseVariables = generador.GenerarClase("CVariables", string.Empty, claseEscenario, "Variables del escenario", MemberAttributes.Public);
            generador.GenerarPropiedadReadOnly(ref claseVariables, "Codigo", string.Empty, "Código de la vista", MemberAttributes.Public, typeof(string).ToString(), true);

            // Constructor de la clase Variables con la asignación del código
            CodeStatement asignacionCodigoVariables = generador.Asignacion("_Codigo", "codigo");
            generador.GenerarConstructor(claseVariables, "Constructor de la clase", MemberAttributes.Public,
                new CodeParameterDeclarationExpression[] { new CodeParameterDeclarationExpression(typeof(string), "codigo") },
                new CodeExpression[] { },
                new CodeStatement[] { asignacionCodigoVariables });

            // Lectura de los alias de la base de datos
            DataTable dtAliasVariables = Orbita.VAControl.AppBD.GetAliasVistaVariables(codVista);
            foreach (DataRow drAlias in dtAliasVariables.Rows)
            {
                // Creamos una propiedad con cada uno de los alias
                string codAlias = drAlias["CodAlias"].ToString();
                string descVariable = drAlias["DescVariable"].ToString();
                generador.GenerarPropiedadReadOnlyRuntime(ref claseVariables, codAlias, string.Empty, descVariable, MemberAttributes.Public, "VariableItem", "VariableRuntime", "GetVariable", new CodeExpression[] { new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "Codigo"), new CodePrimitiveExpression(codAlias) });
            }

            generador.Region("Variables del escenario", claseVariables);
        }

        /// <summary>
        /// Generación de la clase interior al escenario que implementa las variables
        /// </summary>
        /// <param name="generador"></param>
        /// <param name="codVista"></param>
        /// <param name="claseEscenario"></param>
        private static void GenerarHardware(OGeneradorAccesoRapido generador, string codVista, CodeTypeDeclaration claseEscenario)
        {
            // Se genera la propiedad Hardware
            generador.GenerarPropiedadReadOnly(ref claseEscenario, "Hardware", string.Empty, "Hardware del escenario", MemberAttributes.Public, "CHardware", true);

            // Generador de la clase interna de vistas de Hardware
            CodeTypeDeclaration claseHardware = generador.GenerarClase("CHardware", string.Empty, claseEscenario, "Hardware del escenario", MemberAttributes.Public);
            generador.GenerarPropiedadReadOnly(ref claseHardware, "Codigo", string.Empty, "Código de la vista", MemberAttributes.Public, typeof(string).ToString(), true);

            // Constructor de la clase Hardware con la asignación del código
            CodeStatement asignacionCodigoHardware = generador.Asignacion("_Codigo", "codigo");
            generador.GenerarConstructor(claseHardware, "Constructor de la clase", MemberAttributes.Public,
                new CodeParameterDeclarationExpression[] { new CodeParameterDeclarationExpression(typeof(string), "codigo") },
                new CodeExpression[] { },
                new CodeStatement[] { asignacionCodigoHardware });

            // Lectura de los alias de la base de datos
            DataTable dtAliasHardware = Orbita.VAHardware.AppBD.GetAliasVistaHardware(codVista);
            foreach (DataRow drAlias in dtAliasHardware.Rows)
            {
                // Creamos una propiedad con cada uno de los alias
                string codAlias = drAlias["CodAlias"].ToString();
                string descHardware = drAlias["DescHardware"].ToString();
                string claseImplementadoraHardware = drAlias["ClaseImplementadora"].ToString();
                generador.GenerarPropiedadReadOnlyRuntime(ref claseHardware, codAlias, claseImplementadoraHardware, descHardware, MemberAttributes.Public, claseImplementadoraHardware, "HardwareRuntime", "GetHardware", new CodeExpression[] { new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "Codigo"), new CodePrimitiveExpression(codAlias) });
            }

            generador.Region("Hardware del escenario", claseHardware);
        }

        /// <summary>
        /// Generador de estados de una determinada máquina de estados
        /// </summary>
        /// <param name="generador"></param>
        /// <param name="codMaquinaEstados"></param>
        private static bool GenerarEstados(OGeneradorAccesoRapido generador, string codMaquinaEstados, string claseImplementadoraEscenarioAsociado)
        {
            // Lectura de la base de datos
            DataTable dtEstados = Orbita.VAControl.AppBD.GetEstados(codMaquinaEstados);

            // Para cada estado se crea su clase
            foreach (DataRow drEstado in dtEstados.Rows)
            {
                string codEstado = drEstado["CodEstado"].ToString();
                string descEstado = "Estado: " + drEstado["DescEstado"].ToString();
                string claseImplementadora = drEstado["ClaseImplementadora"].ToString();
                OTipoEstado tipoEstado = OEnumRobusto<OTipoEstado>.Validar(drEstado["Tipo"].ToString(), OTipoEstado.EstadoSimple);
                //OTipoEstado tipoEstado = (OTipoEstado)App.EnumParse(typeof(OTipoEstado), drEstado["Tipo"].ToString(), OTipoEstado.EstadoSimple);

                // Generación de la clase parcial del estado
                string claseEstadoPadre;
                switch (tipoEstado)
                {
                    case OTipoEstado.EstadoSimple:
                    default:
                        claseEstadoPadre = typeof (OEstadoBase).Name; //"OEstadoBase";
                        break;
                    case OTipoEstado.EstadoAsincrono:
                        claseEstadoPadre = typeof (OEstadoAsincrono).Name; //"OEstadoAsincrono";
                        break;
                }
                CodeTypeDeclaration claseEstado = generador.GenerarClase(claseImplementadora, claseEstadoPadre, descEstado, MemberAttributes.Public, true);
                if (claseEstado == null)
                {
                    return false;
                }

                // Generadora de constructor con la instrucción new variable
                generador.GenerarConstructor(claseEstado, "Constructor de la clase", MemberAttributes.Public,
                    new CodeParameterDeclarationExpression[] 
                        { new CodeParameterDeclarationExpression(typeof(string), "codigoMaquinaEstados"),
                          new CodeParameterDeclarationExpression(typeof(string), "codigo"),
                          new CodeParameterDeclarationExpression(typeof(OEscenario), "escenario")
                        },
                    new CodeExpression[] 
                        { new CodeVariableReferenceExpression("codigoMaquinaEstados"),
                          new CodeVariableReferenceExpression("codigo"),
                          new CodeVariableReferenceExpression("escenario")
                        },
                    new CodeStatement[] { });

                // Se genera la propiedad Escenarios
                generador.GenerarPropiedadRuntime(ref claseEstado, "Escenario", claseImplementadoraEscenarioAsociado, "Escenario del estado", MemberAttributes.Public | MemberAttributes.New, claseImplementadoraEscenarioAsociado, "_Escenario");

                generador.Region(descEstado, claseEstado);
            }

            return true;
        }

        /// <summary>
        /// Generador de transiciones de una determinada máquina de estados
        /// </summary>
        /// <param name="generador"></param>
        /// <param name="codMaquinaEstados"></param>
        private static bool GenerarTransiciones(OGeneradorAccesoRapido generador, string codMaquinaEstados, string claseImplementadoraEscenarioAsociado, out CodeTypeDeclaration ultimaTransicion)
        {
            ultimaTransicion = null;

            // Lectura de la base de datos
            DataTable dtTransiciones = Orbita.VAControl.AppBD.GetTransiciones(codMaquinaEstados);

            // Para cada transición se crea su clase
            foreach (DataRow drTransicion in dtTransiciones.Rows)
            {
                string codTransicion = drTransicion["CodTransicion"].ToString();
                string descTransicion = "Transición: " + drTransicion["ExplicacionCondicionEsperada"].ToString();
                string claseImplementadora = drTransicion["ClaseImplementadora"].ToString();
                OTipoTransicion tipoTransicion = OEnumRobusto<OTipoTransicion>.Validar(drTransicion["Tipo"].ToString(), OTipoTransicion.TransicionSimple);
                //OTipoTransicion tipoTransicion = (OTipoTransicion)App.EnumParse(typeof(OTipoTransicion), drTransicion["Tipo"].ToString(), OTipoTransicion.TransicionSimple);

                // Generación de la clase parcial del transicion
                string claseTransicionPadre;
                switch (tipoTransicion)
                {
                    case OTipoTransicion.TransicionSimple:
                    default:
                        claseTransicionPadre = typeof (OTransicionBase).Name; //"OTransicionBase";
                        break;
                    case OTipoTransicion.TransicionUniversal:
                        claseTransicionPadre = typeof (OTransicionUniversal).Name; //"OTransicionUniversal";
                        break;
                }
                CodeTypeDeclaration claseTransicion = generador.GenerarClase(claseImplementadora, claseTransicionPadre, descTransicion, MemberAttributes.Public, true);
                if (claseTransicion == null)
                {
                    return false;
                }

                ultimaTransicion = claseTransicion;

                // Generadora de constructor con la instrucción new variable
                generador.GenerarConstructor(claseTransicion, "Constructor de la clase", MemberAttributes.Public,
                    new CodeParameterDeclarationExpression[] 
                        { new CodeParameterDeclarationExpression(typeof(string), "codigoMaquinaEstados"),
                          new CodeParameterDeclarationExpression(typeof(string), "codigo"),
                          new CodeParameterDeclarationExpression(typeof(OEscenario), "escenario")
                        },
                    new CodeExpression[] 
                        { new CodeVariableReferenceExpression("codigoMaquinaEstados"),
                          new CodeVariableReferenceExpression("codigo"),
                          new CodeVariableReferenceExpression("escenario")
                        },
                    new CodeStatement[] { });

                // Se genera la propiedad Escenarios
                generador.GenerarPropiedadRuntime(ref claseTransicion, "Escenario", claseImplementadoraEscenarioAsociado, "Escenario de la transición", MemberAttributes.Public | MemberAttributes.New, claseImplementadoraEscenarioAsociado, "_Escenario");

                DataTable dtTransicion = Orbita.VAControl.AppBD.GetTransicion(codMaquinaEstados, codTransicion);
                if (dtTransicion.Rows.Count == 1)
                {
                    string codEstadoOrigen = dtTransicion.Rows[0]["CodigoEstadoOrigen"].ToString();
                    string codEstadoDestino = dtTransicion.Rows[0]["CodigoEstadoDestino"].ToString();

                    DataTable dtEstadoOrigen = Orbita.VAControl.AppBD.GetEstado(codMaquinaEstados, codEstadoOrigen);
                    if (dtEstadoOrigen.Rows.Count == 1)
                    {
                        // Se genera la propiedad Estado Origen
                        string claseImplementadoraEstado = dtEstadoOrigen.Rows[0]["ClaseImplementadora"].ToString();
                        generador.GenerarPropiedadRuntime(ref claseTransicion, "EstadoOrigen", claseImplementadoraEstado, "Estado origen", MemberAttributes.Public | MemberAttributes.New, claseImplementadoraEstado, "_EstadoOrigen");
                    }                                                                                                                                                                                                   

                    DataTable dtEstadoDestino = Orbita.VAControl.AppBD.GetEstado(codMaquinaEstados, codEstadoDestino);
                    if (dtEstadoDestino.Rows.Count == 1)
                    {
                        // Se genera la propiedad Estado Origen
                        string claseImplementadoraEstado = dtEstadoDestino.Rows[0]["ClaseImplementadora"].ToString();
                        generador.GenerarPropiedadRuntime(ref claseTransicion, "EstadoDestino", claseImplementadoraEstado, "Destino", MemberAttributes.Public | MemberAttributes.New, claseImplementadoraEstado, "_EstadoDestino");
                    }
                }

                generador.Region(descTransicion, claseTransicion);
            }

            return true;
        }
        #endregion
    }
}
