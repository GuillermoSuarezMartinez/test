//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 13-12-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.CSharp;

namespace Orbita.VAComun
{
    /// <summary>
    /// Clase que implementa la ejecución de un script (fichero .cs externo a la aplicación)
    /// </summary>
    public class OScript
    {
        #region Atributo(s)
        /// <summary>
        /// Nombre de la clase del script
        /// </summary>
        private string NombreClase;
        /// <summary>
        /// Espacio de nombres del script (namespace)
        /// </summary>
        private string EspacioNombres;
        /// <summary>
        /// Ruta del código a compilar
        /// </summary>
        private string RutaScript;
        /// <summary>
        /// Ruta del ensamablado compilado
        /// </summary>
        private string RutaEnsamblado;
        /// <summary>
        /// Referencias a incluir en el ensamblado
        /// </summary>
        private List<string> Referencias;
        /// <summary>
        /// Indica si el script ha sido compilado
        /// </summary>
        public bool Compilado;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Código identificativo de la clase.
        /// </summary>
        private string _Codigo;
        /// <summary>
        /// Código identificativo de la clase.
        /// </summary>
        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la calse
        /// </summary>
        /// <param name="codigoScript">Código identificativo del script (tando del espacio de nombres como de la clase)</param>
        /// <param name="rutaScript">Ruta del código a compilar</param>
        /// <param name="rutaEnsamblado">Ruta del ensamablado compilado</param>
        /// <param name="referencias">Referencias a incluir en el ensamblado</param>
        public OScript(string nombreClase, string espacioNombres, string rutaScript, string rutaEnsamblado, List<string> referencias)
        {
            this.Codigo = nombreClase;
            this.NombreClase = nombreClase;
            this.EspacioNombres = espacioNombres;
            this.RutaScript = rutaScript;
            this.RutaEnsamblado = rutaEnsamblado;
            this.Referencias = referencias;
            this.Compilado = false;
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Compila el script creando un nuevo ensamblado .dll
        /// </summary>
        /// <returns>Verdadero si el método ha funcionado con éxito</returns>
        public bool Compilar()
        {
            bool resultado = false;

            if (this.Validar())
            {
                try
                {
                    // Creación del compilador
                    CSharpCodeProvider compiladorCSharp = new CSharpCodeProvider();
                    // Creación de los parámetros del compilador
                    CompilerParameters cp = new CompilerParameters();
                    // Generate an executable instead of a class library.
                    cp.GenerateExecutable = false;
                    // Set the assembly file name to generate.
                    cp.OutputAssembly = this.RutaEnsamblado;
                    // Generate debug information.
                    cp.IncludeDebugInformation = true;
                    // Add an assembly reference.
                    foreach (string referencia in this.Referencias)
                    {
                        cp.ReferencedAssemblies.Add(referencia);
                    }

                    // Save the assembly as a physical file.
                    cp.GenerateInMemory = false;
                    // Set the level at which the compiler should start displaying warnings.
                    cp.WarningLevel = 3;
                    // Set whether to treat all warnings as errors.
                    cp.TreatWarningsAsErrors = false;
                    // Set compiler argument to optimize output.
                    cp.CompilerOptions = "/optimize";
                    // Set a temporary files collection. The TempFileCollection stores the temporary files generated during a build in the current directory, and does not delete them after compilation. 
                    cp.TempFiles = new TempFileCollection(Path.GetTempPath(), false);

                    // Compilación
                    CompilerResults cr = compiladorCSharp.CompileAssemblyFromFile(cp, this.RutaScript);

                    // Verificación
                    this.Compilado = (cr.Errors.Count == 0) && (File.Exists(this.RutaEnsamblado));
                    resultado = this.Compilado;
                }
                catch (Exception exception)
                {
                    OVALogsManager.Error(ModulosSistema.Comun, "Compilar", exception);
                }
            }

            return resultado;
        }

        /// <summary>
        /// Crea una instancia del objeto del script
        /// </summary>
        /// <typeparam name="ClasePadre">Clase padre del objeto creado</typeparam>
        /// <returns>Retorna una instancia de la clase padre del objeto</returns>
        public bool CrearInstancia<ClasePadre>(out ClasePadre instancia)
        {
            instancia = default(ClasePadre);
            if (this.Compilado)
            {
                // Creación de la instancia
                Assembly ensamblado = Assembly.LoadFile(this.RutaEnsamblado);
                Type tipo = ensamblado.GetType(this.EspacioNombres + "." + this.NombreClase);
                if (tipo.IsClass && (tipo.BaseType == typeof(ClasePadre)))
                {
                    instancia = (ClasePadre)ensamblado.CreateInstance(this.EspacioNombres + "." + this.NombreClase);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Se valida que los parámetros de ejecución del script son correctos.
        /// </summary>
        /// <returns></returns>
        public bool Validar()
        {
            bool resultado = true;

            resultado &= NombreClase != string.Empty;
            resultado &= File.Exists(this.RutaScript);
            resultado &= Directory.Exists(Path.GetDirectoryName(this.RutaEnsamblado));

            return resultado;
        }

        #endregion
    }
}
