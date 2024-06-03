//***********************************************************************
// Assembly         : Orbita.VAControl
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 12-12-2012
// Description      : Adaptada la forma de utilizar el thread
//
// Last Modified By : aibañez
// Last Modified On : 16-11-2012
// Description      : Cambiadas referencias a eventos genéricos de App.cs
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters;
using System.Security.Permissions;
using System.Threading;
using Orbita.VAComun;

namespace Orbita.VAControl
{
    /// <summary>
    /// Clase estática para acceder a las variables desde cualquier lugar de la aplicacion
    /// </summary>
    public static class OVariablesManager
    {
        #region Atributo(s)

        /// <summary>
        /// Indica si la clase estática está iniciada
        /// </summary>
        public static bool Iniciado = false;

        /// <summary>
        /// Lista de todas las variables del sistema
        /// </summary>
        public static Dictionary<string, OVariable> ListaVariables;

        /// <summary>
        /// Lista de todas las vistas de variables del sistema
        /// </summary>
        public static Dictionary<string, OVistaVariable> Vistas;

        /// <summary>
        /// Variable que almacena la trazabilidad de las variables
        /// </summary>
        private static OTrazabilidadVariables Trazabilidad;

        /// <summary>
        /// Tiempo que permanecen las trazas en memoria
        /// </summary>
        public static TimeSpan TiempoPermanenciaTrazasEnMemoria;

        /// <summary>
        /// Indica la frecuencia con la que se monitorizan las variables (la variable ha de ser divisible por TiempoPermanenciaTrazasEnMemoria)
        /// </summary>
        public static TimeSpan CadenciaMonitorizacion;

        /// <summary>
        /// Puerto de comunicación con la variable remota
        /// </summary>
        internal static int PuertoRemoto;

        /// <summary>
        /// Canal de Servidor de remoting
        /// </summary>
        internal static TcpChannel CanalServidor; //channel to communicate

        #endregion

        #region Método(s) público(s)

        /// <summary>
        /// Construye los objetos
        /// </summary>
        public static void Constructor()
        {
            // Creación de los objetos
            ListaVariables = new Dictionary<string, OVariable>();
            Vistas = new Dictionary<string, OVistaVariable>();
            Trazabilidad = new OTrazabilidadVariables();

            CadenciaMonitorizacion = OSistemaManager.Configuracion.CadenciaMonitorizacion;
            TiempoPermanenciaTrazasEnMemoria = TimeSpan.FromMilliseconds(60000); //¿?¿?¿?
            PuertoRemoto = 8085;

            // Consulta a la base de datos
            DataTable dtSistema = Orbita.VAComun.AppBD.GetParametrosAplicacion();
            if (dtSistema.Rows.Count > 0)
            {
                object objTiempoPermanenciaTrazasEnMemoria = dtSistema.Rows[0]["VarsPermanenciaTrazaMemoria"];
                TiempoPermanenciaTrazasEnMemoria = TimeSpan.FromMilliseconds(OEnteroRobusto.Validar(objTiempoPermanenciaTrazasEnMemoria, 1, 86400000, 60000));

                PuertoRemoto = (int)OEnteroRobusto.Validar(dtSistema.Rows[0]["VarsPuertoRemoting"], 1, 65535, 8085);
            }
            else
            {
                throw new Exception("No existe nigún registro de parametrización de la aplicación en la base de datos");
            }

            // Registro del canal de servidor
            //ORemoting.InicConfiguracionServidor("Orbita.VAControl", "GetRemoteVariableCore", PuertoRemoto, "VarsServidor");

            BinaryClientFormatterSinkProvider clientProvider = null;
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
            IDictionary props = new Hashtable();
            props["port"] = PuertoRemoto;
            props["typeFilterLevel"] = TypeFilterLevel.Full;
            props["name"] = "VarsServidor";
            CanalServidor = new TcpChannel(props, clientProvider, serverProvider); //channel to communicate
            ChannelServices.RegisterChannel(CanalServidor, false);  //register channel
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(OGetRemoteVariableCore), "Vars", WellKnownObjectMode.Singleton); //register remote object

            // Consulta de todas las variables existentes en el sistema
            DataTable dtVar = AppBD.GetVariables();
            if (dtVar.Rows.Count > 0)
            {
                // Cargamos todas las variables existentes en el sistema
                OVariable varItem;
                foreach (DataRow drVar in dtVar.Rows)
                {
                    // Creamos cada una de las variables del sistema
                    string codVariable = drVar["CodVariable"].ToString();
                    varItem = new OVariable(codVariable);
                    ListaVariables.Add(codVariable, varItem);
                }
            }

            // Consulta de todas las vistas existentes en el sistema
            DataTable dtVista = Orbita.VAComun.AppBD.GetVistas();
            if (dtVista.Rows.Count > 0)
            {
                // Cargamos todas las vistas existentes en el sistema
                OVistaVariable vista;
                foreach (DataRow drVista in dtVista.Rows)
                {
                    // Creamos cada una de las vistas del sistema
                    string codVista = drVista["CodVista"].ToString();
                    vista = new OVistaVariable(codVista);
                    Vistas.Add(codVista, vista);
                }
            }
        }

        /// <summary>
        /// Destruye los objetos
        /// </summary>
        public static void Destructor()
        {
            // Destrucción de los objetos
            ListaVariables = null;
            Trazabilidad = null;
            ChannelServices.UnregisterChannel(CanalServidor);
        }

        /// <summary>
        /// Se cargan los valores de la clase
        /// </summary>
        public static void Inicializar()
        {
            foreach (OVariable varItem in ListaVariables.Values)
            {
                varItem.Inicializar();
            }

            Trazabilidad.Inicializar();

            Iniciado = true;
        }

        /// <summary>
        /// Se finaliza la ejecución
        /// </summary>
        public static void Finalizar()
        {
            Iniciado = false;

            Trazabilidad.Finalizar();

            foreach (OVariable varItem in ListaVariables.Values)
            {
                varItem.Finalizar();
            }
        }

        /// <summary>
        /// Método para acceder al valor de una variable
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <returns>Devuelve el valor de la variable con el código correspondientes</returns>
        public static object GetValue(string codigo)
        {
            return GetValue(string.Empty, codigo);
        }
        /// <summary>
        /// Método para acceder al valor de una variable
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <returns>Devuelve el valor de la variable con el código correspondientes</returns>
        public static object GetValue(string vista, string codigo)
        {
            OVariable variable;
            if (TryGetVariable(vista, codigo, out variable))
            {
                return variable.GetValor();
            }

            return null;
        }

        /// <summary>
        /// Método para consultar el tiempo de permanencia del valor de la variable
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <returns>Devuelve el tiempo de permanencia del valor de la variable</returns>
        public static TimeSpan GetPermanencia(string codigo)
        {
            return GetPermanencia(string.Empty, codigo);
        }
        /// <summary>
        /// Método para consultar el tiempo de permanencia del valor de la variable
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <returns>Devuelve el tiempo de permanencia del valor de la variable</returns>
        public static TimeSpan GetPermanencia(string vista, string codigo)
        {
            OVariable variable;
            if (TryGetVariable(vista, codigo, out variable))
            {
                return variable.GetPermanencia();
            }

            return TimeSpan.FromMilliseconds(0);
        }


        /// <summary>
        /// Método para comprobar si el valor de la variable ha cambiado
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <returns>Devuelve verdadero si valor de la variable con el código correspondientes ha cambiado</returns>
        public static bool GetChanged(string codigo, string codRemitente)
        {
            return GetChanged(string.Empty, codigo, codRemitente);
        }
        /// <summary>
        /// Método para comprobar si el valor de la variable ha cambiado
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <returns>Devuelve verdadero si valor de la variable con el código correspondientes ha cambiado</returns>
        public static bool GetChanged(string vista, string codigo, string codRemitente)
        {
            OVariable variable;
            if (TryGetVariable(vista, codigo, out variable))
            {
                return variable.GetHayCambio(codRemitente);
            }

            return false;
        }

        /// <summary>
        /// Método para comprobar si el valor de la variable ha cambiado
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <returns>Devuelve verdadero si valor de la variable con el código correspondientes ha cambiado</returns>
        public static OEnumTipoDato GetType(string codigo)
        {
            return GetType(string.Empty, codigo);
        }
        /// <summary>
        /// Método para comprobar si el valor de la variable ha cambiado
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <returns>Devuelve verdadero si valor de la variable con el código correspondientes ha cambiado</returns>
        public static OEnumTipoDato GetType(string vista, string codigo)
        {
            OVariable variable;
            if (TryGetVariable(vista, codigo, out variable))
            {
                return variable.GetTipo();
            }

            return OEnumTipoDato.SinDefinir;
        }

        /// <summary>
        /// Método para modificar el valor de una variable a de forma registrada
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="valor">Nuevo valor de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void SetValue(string codigo, object valor, string codigoModuloLlamada, string descripcionLlamada)
        {
            SetValue(string.Empty, codigo, valor, codigoModuloLlamada, descripcionLlamada);
        }
        /// <summary>
        /// Método para modificar el valor de una variable a de forma registrada
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="valor">Nuevo valor de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void SetValue(string vista, string codigo, object valor, string codigoModuloLlamada, string descripcionLlamada)
        {
            OVariable variable;
            if (TryGetVariable(vista, codigo, out variable))
            {
                variable.SetValor(valor, codigoModuloLlamada, descripcionLlamada);
            }
        }

        /// <summary>
        /// Método para modificar el valor de una variable a de forma registrada
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="valor">Nuevo valor de la variable</param>
        /// <param name="retraso">Tiempo de retraso de la actualización del valor</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void SetValueDelayed(string codigo, object valor, TimeSpan retraso, string codigoModuloLlamada, string descripcionLlamada)
        {
            SetValueDelayed(string.Empty, codigo, valor, retraso, codigoModuloLlamada, descripcionLlamada);
        }
        /// <summary>
        /// Método para modificar el valor de una variable a de forma registrada
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="valor">Nuevo valor de la variable</param>
        /// <param name="retraso">Tiempo de retraso de la actualización del valor</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void SetValueDelayed(string vista, string codigo, object valor, TimeSpan retraso, string codigoModuloLlamada, string descripcionLlamada)
        {
            OVariable variable;
            if (TryGetVariable(vista, codigo, out variable))
            {
                variable.SetValorRetrasado(valor, retraso, codigoModuloLlamada, descripcionLlamada);
            }
        }

        /// <summary>
        /// Método para modificar el valor de una variable a de forma registrada y bloquearla para que no se modifique
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="valor">Nuevo valor de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void Bloquear(string codigo, string codigoModuloLlamada, string descripcionLlamada)
        {
            Bloquear(string.Empty, codigo, codigoModuloLlamada, descripcionLlamada);
        }
        /// <summary>
        /// Método para modificar el valor de una variable a de forma registrada y bloquearla para que no se modifique
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="valor">Nuevo valor de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void Bloquear(string vista, string codigo, string codigoModuloLlamada, string descripcionLlamada)
        {
            OVariable variable;
            if (TryGetVariable(vista, codigo, out variable))
            {
                variable.Bloquear(codigoModuloLlamada, descripcionLlamada);
            }
        }

        /// <summary>
        /// Método para desbloquear una variable y que cualquiera pueda modificarla
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void Desbloquear(string codigo, string codigoModuloLlamada, string descripcionLlamada)
        {
            Desbloquear(string.Empty, codigo, codigoModuloLlamada, descripcionLlamada);
        }
        /// <summary>
        /// Método para desbloquear una variable y que cualquiera pueda modificarla
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void Desbloquear(string vista, string codigo, string codigoModuloLlamada, string descripcionLlamada)
        {
            OVariable variable;
            if (TryGetVariable(vista, codigo, out variable))
            {
                variable.Desbloquear(codigoModuloLlamada, descripcionLlamada);
            }
        }

        /// <summary>
        /// Método para inhibir la modificación del valor de una variable
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void Inhibir(string codigo, string codigoModuloLlamada, string descripcionLlamada)
        {
            Inhibir(string.Empty, codigo, codigoModuloLlamada, descripcionLlamada);
        }
        /// <summary>
        /// Método para inhibir la modificación del valor de una variable
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void Inhibir(string vista, string codigo, string codigoModuloLlamada, string descripcionLlamada)
        {
            OVariable variable;
            if (TryGetVariable(vista, codigo, out variable))
            {
                variable.Inhibir(codigoModuloLlamada, descripcionLlamada);
            }
        }

        /// <summary>
        /// Método para desinhibir la modificación del valor de una variable
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void Desinhibir(string codigo, string codigoModuloLlamada, string descripcionLlamada)
        {
            Desinhibir(string.Empty, codigo, codigoModuloLlamada, descripcionLlamada);
        }
        /// <summary>
        /// Método para desinhibir la modificación del valor de una variable
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void Desinhibir(string vista, string codigo, string codigoModuloLlamada, string descripcionLlamada)
        {
            OVariable variable;
            if (TryGetVariable(vista, codigo, out variable))
            {
                variable.Desinhibir(codigoModuloLlamada, descripcionLlamada);
            }
        }

        /// <summary>
        /// Método para modificar el valor de una variable a de forma registrada cuando está bloqueada
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="valor">Nuevo valor de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void ForzarValor(string codigo, object valor, string codigoModuloLlamada, string descripcionLlamada)
        {
            ForzarValor(string.Empty, codigo, valor, codigoModuloLlamada, descripcionLlamada);
        }
        /// <summary>
        /// Método para modificar el valor de una variable a de forma registrada cuando está bloqueada
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="valor">Nuevo valor de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void ForzarValor(string vista, string codigo, object valor, string codigoModuloLlamada, string descripcionLlamada)
        {
            OVariable variable;
            if (TryGetVariable(vista, codigo, out variable))
            {
                variable.ForzarValor(valor, codigoModuloLlamada, descripcionLlamada);
            }
        }

        /// <summary>
        /// Obtiene la variable de un determinado código
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <returns>variable</returns>
        public static OVariable GetVariable(string codigo)
        {
            return GetVariable(string.Empty, codigo);
        }
        /// <summary>
        /// Obtiene la variable de un determinado código
        /// </summary>
        /// <param name="vista">Vista utilizada</param>
        /// <param name="codigo">Código de la variable</param>
        /// <returns>variable</returns>
        public static OVariable GetVariable(string vista, string codigo)
        {
            OVariable variable;
            if (TryGetVariable(vista, codigo, out variable))
            {
                return variable;
            }

            return null;
        }

        /// <summary>
        /// Método para disparar las suscripciones de una variable a de forma registrada
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void Dispara(string codigo, string codigoModuloLlamada, string descripcionLlamada)
        {
            Dispara(string.Empty, codigo, codigoModuloLlamada, descripcionLlamada);
        }
        /// <summary>
        /// Método para disparar las suscripciones de una variable a de forma registrada
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void Dispara(string vista, string codigo, string codigoModuloLlamada, string descripcionLlamada)
        {
            OVariable variable;
            if (TryGetVariable(vista, codigo, out variable))
            {
                variable.Disparo(codigoModuloLlamada, descripcionLlamada);
            }
        }

        /// <summary>
        /// Suscribe a una determinada variable
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void CrearSuscripcion(string codigo, string codigoModuloLlamada, string descripcionLlamada, OCambioValorDelegate delegadoSuscriptor)
        {
            CrearSuscripcion(string.Empty, codigo, codigoModuloLlamada, descripcionLlamada, delegadoSuscriptor);
        }
        /// <summary>
        /// Suscribe a una determinada variable
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void CrearSuscripcion(string vista, string codigo, string codigoModuloLlamada, string descripcionLlamada, OCambioValorDelegate delegadoSuscriptor)
        {
            OVariable variable;
            if (TryGetVariable(vista, codigo, out variable))
            {
                variable.CrearSuscripcion(codigoModuloLlamada, descripcionLlamada, delegadoSuscriptor);
            }
        }

        /// <summary>
        /// Suscribe a una determinada variable
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void CrearSuscripcion(string codigo, string codigoModuloLlamada, string descripcionLlamada, OCambioValorDelegateAdvanced delegadoSuscriptor)
        {
            CrearSuscripcion(string.Empty, codigo, codigoModuloLlamada, descripcionLlamada, delegadoSuscriptor);
        }
        /// <summary>
        /// Suscribe a una determinada variable
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void CrearSuscripcion(string vista, string codigo, string codigoModuloLlamada, string descripcionLlamada, OCambioValorDelegateAdvanced delegadoSuscriptor)
        {
            OVariable variable;
            if (TryGetVariable(vista, codigo, out variable))
            {
                variable.CrearSuscripcion(codigoModuloLlamada, descripcionLlamada, delegadoSuscriptor);
            }
        }

        /// <summary>
        /// Elimina la suscripción a una determinada variable
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void EliminarSuscripcion(string codigo, string codigoModuloLlamada, string descripcionLlamada, OCambioValorDelegate delegadoSuscriptor)
        {
            EliminarSuscripcion(string.Empty, codigo, codigoModuloLlamada, descripcionLlamada, delegadoSuscriptor);
        }
        /// <summary>
        /// Elimina la suscripción a una determinada variable
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void EliminarSuscripcion(string vista, string codigo, string codigoModuloLlamada, string descripcionLlamada, OCambioValorDelegate delegadoSuscriptor)
        {
            OVariable variable;
            if (TryGetVariable(vista, codigo, out variable))
            {
                variable.EliminarSuscripcion(codigoModuloLlamada, descripcionLlamada, delegadoSuscriptor);
            }
        }

        /// <summary>
        /// Elimina la suscripción a una determinada variable
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void EliminarSuscripcion(string codigo, string codigoModuloLlamada, string descripcionLlamada, OCambioValorDelegateAdvanced delegadoSuscriptor)
        {
            EliminarSuscripcion(string.Empty, codigo, codigoModuloLlamada, descripcionLlamada, delegadoSuscriptor);
        }
        /// <summary>
        /// Elimina la suscripción a una determinada variable
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        public static void EliminarSuscripcion(string vista, string codigo, string codigoModuloLlamada, string descripcionLlamada, OCambioValorDelegateAdvanced delegadoSuscriptor)
        {
            OVariable variable;
            if (TryGetVariable(vista, codigo, out variable))
            {
                variable.EliminarSuscripcion(codigoModuloLlamada, descripcionLlamada, delegadoSuscriptor);
            }
        }

        /// <summary>
        /// Añade una nueva traza a la cola
        /// </summary>
        /// <param name="traza"></param>
        internal static void NuevaTraza(OTrazaVariable traza)
        {
            Trazabilidad.NuevaTraza(traza);
        }

        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Método para acceder a una variable
        /// </summary>
        /// <param name="codigo">Vista</param>
        /// <param name="codigo">Código o alias de la variable</param>
        /// <returns>Devuelve la variable correspondientes</returns>
        private static bool TryGetVariable(string codVista, string codAlias, out OVariable variableItem)
        {
            // Inicialización de resultados
            string alias = codAlias;

            // Cambio el alias al de la vista
            if (codVista != string.Empty)
            {
                // Cambio el alias
                OVistaVariable vistaVariable;
                if (Vistas.TryGetValue(codVista, out vistaVariable))
                {
                    string codVariable;
                    if (vistaVariable.ListaAlias.TryGetValue(codAlias, out codVariable))
                    {
                        alias = codVariable;
                    }
                }
            }

            return ListaVariables.TryGetValue(alias, out variableItem);
        }
        #endregion
    }

    /// <summary>
    /// Clase que implementa las vistas de las variables.
    /// Las vistas son agrupaciones de variables que se acceden con un alias en lugar de por su código
    /// </summary>
    public class OVistaVariable
    {
        #region Atributo(s)
        /// <summary>
        /// Código de la vista
        /// </summary>
        public string Codigo;

        /// <summary>
        /// Lista de todos los alias de la vista
        /// </summary>
        public Dictionary<string, string> ListaAlias;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codVista">Código de la vista</param>
        public OVistaVariable(string codVista)
        {
            this.Codigo = codVista;

            this.ListaAlias = new Dictionary<string, string>();
            this.RellenarAlias(codVista);
        }

        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Rellena la lista de alias de la vista
        /// </summary>
        private void RellenarAlias(string codVista)
        {
            // Consulta de todos los alias existentes en el sistema
            DataTable dtVar = AppBD.GetAliasVistaVariables(codVista);
            if (dtVar.Rows.Count > 0)
            {
                // Cargamos todos los alias existentes en el sistema
                foreach (DataRow drVar in dtVar.Rows)
                {
                    // Creamos cada una de las variables del sistema
                    string codAlias = drVar["CodAlias"].ToString();
                    string codVariable = drVar["CodVariable"].ToString();
                    ListaAlias.Add(codAlias, codVariable);
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// Clase que contiene un VariableCore que puede ser local o remota.
    /// </summary>
    [Serializable]
    public class OVariable
    {
        #region Atributo(s)
        /// <summary>
        /// Nucleo de la variable
        /// </summary>
        internal VariableCore VariableCore;
        /// <summary>
        /// Canal del ciente
        /// </summary>
        private TcpChannel CanalCliente;
        /// <summary>
        /// Objeto utilizado para enlazar con los eventos del variablecore de forma remota
        /// </summary>
        private OVariableBroadcastEventWraper EventWrapper;
        #endregion

        #region Propiedad(es)

        /// <summary>
        /// Código identificativo de la variable
        /// </summary>
        private string _Codigo;
        /// <summary>
        /// Código identificativo de la variable
        /// </summary>
        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }

        /// <summary>
        /// Código identificativo del grupo
        /// </summary>
        private string _Grupo;
        /// <summary>
        /// Código identificativo del grupo
        /// </summary>
        public string Grupo
        {
            get { return _Grupo; }
            set { _Grupo = value; }
        }

        /// <summary>
        /// Nombre identificativo de la variable
        /// </summary>
        private string _Nombre;
        /// <summary>
        /// Nombre identificativo de la variable
        /// </summary>
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        /// <summary>
        /// Descripción de la variable
        /// </summary>
        private string _Descripcion;
        /// <summary>
        /// Descripción de la variable
        /// </summary>
        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }

        /// <summary>
        /// Indica que se la variable pertenece a otra aplicación
        /// </summary>
        private bool _Remoto;
        /// <summary>
        /// Indica que se la variable pertenece a otra aplicación
        /// </summary>
        public bool Remoto
        {
            get { return _Remoto; }
            set { _Remoto = value; }
        }

        /// <summary>
        /// Dirección del servidor de la variable remota
        /// </summary>
        private string _ServidorRemoto;
        /// <summary>
        /// Dirección del servidor de la variable remota
        /// </summary>
        public string ServidorRemoto
        {
            get { return _ServidorRemoto; }
            set { _ServidorRemoto = value; }
        }

        /// <summary>
        /// Código de la variable remota
        /// </summary>
        private string _CodigoRemoto;
        /// <summary>
        /// Código de la variable remota
        /// </summary>
        public string CodigoRemoto
        {
            get { return _CodigoRemoto; }
            set { _CodigoRemoto = value; }
        }

        /// <summary>
        /// Puerto de la variable remota
        /// </summary>
        private int _PuertoRemoto;
        /// <summary>
        /// Puerto de la variable remota
        /// </summary>
        public int PuertoRemoto
        {
            get { return _PuertoRemoto; }
            set { _PuertoRemoto = value; }
        }

        /// <summary>
        /// Define si la variable puede modificarse o tiene fijado su valor
        /// </summary>
        public bool Bloqueado
        {
            get { return this.VariableCore.Bloqueado; }
            set { this.VariableCore.Bloqueado = value; }
        }

        /// <summary>
        /// Define si la variable puede modificarse o tiene fijado su valor
        /// </summary>
        public bool Inhibido
        {
            get { return this.VariableCore.Inhibido; }
            set { this.VariableCore.Inhibido = value; }
        }

        /// <summary>
        /// Habilita o deshabilita el funcionamiento
        /// </summary>
        public bool Habilitado
        {
            get { return this.VariableCore.Habilitado; }
            set { this.VariableCore.Habilitado = value; }

        }

        /// <summary>
        /// Indica que se ha de guardar la trazabilidad en la base de datos
        /// </summary>
        public bool GuardarTrazabilidad
        {
            get { return this.VariableCore.GuardarTrazabilidad; }
            set { this.VariableCore.GuardarTrazabilidad = value; }
        }

        /// <summary>
        /// Tipo de la variable
        /// </summary>
        public OEnumTipoDato Tipo
        {
            get { return this.VariableCore.Tipo; }
            set { this.VariableCore.Tipo = value; }
        }

        #endregion

        #region Constructor(es)

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OVariable(string codigo)
        {
            // Inicialiamos los valores
            this._Codigo = codigo;

            // Cargamos valores de la base de datos
            DataTable dtVariable = AppBD.GetVariable(this._Codigo);
            if (dtVariable.Rows.Count == 1)
            {
                this._Nombre = dtVariable.Rows[0]["NombreVariable"].ToString();
                this._Descripcion = dtVariable.Rows[0]["DescVariable"].ToString();
                this._Grupo = dtVariable.Rows[0]["Grupo"].ToString();
                this._Remoto = OBoolRobusto.Validar(dtVariable.Rows[0]["Remoto"], false);
                this._ServidorRemoto = dtVariable.Rows[0]["ServidorRemoto"].ToString();
                this._CodigoRemoto = dtVariable.Rows[0]["CodigoRemoto"].ToString();
                this._PuertoRemoto = (int)OEnteroRobusto.Validar(dtVariable.Rows[0]["PuertoRemoto"], 1, 65535, 8085);

                bool habilitado = (bool)dtVariable.Rows[0]["HabilitadoVariable"];
                OEnumTipoDato tipo = (OEnumTipoDato)OEnteroRobusto.Validar(dtVariable.Rows[0]["IdTipoVariable"], 0, 99, 0);

                bool guardarTrazabilidad = (bool)dtVariable.Rows[0]["GuardarTrazabilidad"];

                if (this.Remoto) // Cliente Remoting
                {
                    bool yaRegistrado = false;
                    foreach (IChannel canal in ChannelServices.RegisteredChannels)
                    {
                        if (canal.ChannelName == "VarsCliente")
                        {
                            yaRegistrado = true;
                            break;
                        }
                    }

                    if (!yaRegistrado)
                    {
                        BinaryClientFormatterSinkProvider clientProvider2 = new BinaryClientFormatterSinkProvider();
                        BinaryServerFormatterSinkProvider serverProvider2 = new BinaryServerFormatterSinkProvider();
                        serverProvider2.TypeFilterLevel = TypeFilterLevel.Full;
                        IDictionary props2 = new Hashtable();
                        props2["port"] = 0;
                        props2["typeFilterLevel"] = TypeFilterLevel.Full;
                        props2["name"] = "VarsCliente";
                        this.CanalCliente = new TcpChannel(props2, clientProvider2, serverProvider2);
                        this.CanalCliente.StartListening(0);
                        ChannelServices.RegisterChannel(this.CanalCliente, false);  //register channel
                    }

                    string direccion = string.Format(@"tcp://{0}:{1}/{2}", this.ServidorRemoto, this._PuertoRemoto, "Vars"); // Dirección remota
                    OGetRemoteVariableCore getRemoteVariableCore = (OGetRemoteVariableCore)Activator.GetObject(typeof(OGetRemoteVariableCore), direccion);
                    this.VariableCore = getRemoteVariableCore.GetVariableCore(this.CodigoRemoto);

                    // Eventos
                    this.EventWrapper = new OVariableBroadcastEventWraper();
                    this.EventWrapper.CambioValor += this.EjecutaEventos;
                    this.VariableCore.CambioValorRemoto += this.EventWrapper.OnCambioValor;
                }
                else // Servidor Remoting
                {
                    this.VariableCore = new VariableCore(this._Codigo, habilitado, tipo, guardarTrazabilidad);
                    this.VariableCore.CambioValor += this.EjecutaEventos;
                }
            }
        }

        #endregion

        #region Método(s) público(s)

        /// <summary>
        /// Carga los valores de la variable
        /// </summary>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void Inicializar()
        {
            this.VariableCore.Inicializar();
            //this.VariableCore.CambioValor += this.EjecutaEventos;
        }

        /// <summary>
        /// Finaliza la ejecución
        /// </summary>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void Finalizar()
        {
            if (this.Remoto)
            {
                this.VariableCore.CambioValorRemoto -= this.EventWrapper.OnCambioValor;
                this.EventWrapper.CambioValor -= this.EjecutaEventos;
                ChannelServices.UnregisterChannel(this.CanalCliente);
            }
            else
            {
                this.VariableCore.CambioValor -= this.EjecutaEventos;
                this.VariableCore.Finalizar();
            }
        }

        /// <summary>
        /// Lectura de la variable
        /// </summary>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public object GetValor()
        {
            if (OVariablesManager.Iniciado)
            {
                return this.VariableCore.GetValor();
            }
            return null;
        }

        /// <summary>
        /// Método para consultar el tiempo de permanencia del valor de la variable
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <returns>Devuelve el tiempo de permanencia del valor de la variable</returns>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public TimeSpan GetPermanencia()
        {
            if (OVariablesManager.Iniciado)
            {
                return this.VariableCore.GetPermanencia();
            }

            return TimeSpan.FromMilliseconds(0);
        }

        /// <summary>
        /// Método para comprobar si el valor de la variable ha cambiado
        /// </summary>
        /// <returns>Devuelve verdadero si valor de la variable con el código correspondientes ha cambiado</returns>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public bool GetHayCambio(string codRemitente)
        {
            if (OVariablesManager.Iniciado)
            {
                return this.VariableCore.GetHayCambio(codRemitente);
            }
            return false;
        }

        /// <summary>
        /// Escritura de la variable
        /// </summary>
        /// <param name="valor">Nuevo valor de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void SetValor(object valor, string codigoModuloLlamada, string descripcionLlamada)
        {
            if (OVariablesManager.Iniciado)
            {
                if (!this.Remoto)
                {
                    this.VariableCore.SetValor(valor, codigoModuloLlamada, descripcionLlamada);
                }
                else
                {
                    // Se lanza desde un thread distino.
                    CambiaValorEnThread cambioValorEnThread = new CambiaValorEnThread(this.VariableCore.SetValor);
                    cambioValorEnThread.BeginInvoke(valor, codigoModuloLlamada, descripcionLlamada, null, null);
                }
            }
        }

        /// <summary>
        /// Escritura de la variable de forma retrasada.
        /// Transcurido el tiempo especificado se modifica su valor al deseado
        /// </summary>
        /// <param name="valor">Nuevo valor de la variable</param>
        /// <param name="retraso">Tiempo de retraso de la actualización del valor</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void SetValorRetrasado(object valor, TimeSpan retraso, string codigoModuloLlamada, string descripcionLlamada)
        {
            if (OVariablesManager.Iniciado)
            {
                this.VariableCore.SetValorRetrasado(valor, retraso, codigoModuloLlamada, descripcionLlamada);
            }
        }

        /// <summary>
        /// Método para modificar el valor de una variable a de forma registrada y bloquearla para que no se modifique
        /// </summary>
        /// <param name="valor">Nuevo valor de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void Bloquear(string codigoModuloLlamada, string descripcionLlamada)
        {
            if (OVariablesManager.Iniciado)
            {
                this.VariableCore.Bloquear(codigoModuloLlamada, descripcionLlamada);
            }
        }

        /// <summary>
        /// Método para desbloquear una variable y que cualquiera pueda modificarla
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void Desbloquear(string codigoModuloLlamada, string descripcionLlamada)
        {
            if (OVariablesManager.Iniciado)
            {
                this.VariableCore.Desbloquear(codigoModuloLlamada, descripcionLlamada);
            }
        }

        /// <summary>
        /// Método para inhibir la modificación del valor de una variable
        /// </summary>
        /// <param name="valor">Nuevo valor de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void Inhibir(string codigoModuloLlamada, string descripcionLlamada)
        {
            if (OVariablesManager.Iniciado)
            {
                this.VariableCore.Inhibir(codigoModuloLlamada, descripcionLlamada);
            }
        }

        /// <summary>
        /// Método para desinhibir la modificación del valor de una variable
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void Desinhibir(string codigoModuloLlamada, string descripcionLlamada)
        {
            if (OVariablesManager.Iniciado)
            {
                this.VariableCore.Desinhibir(codigoModuloLlamada, descripcionLlamada);
            }
        }

        /// <summary>
        /// Escritura de la variable cuando está bloqueada
        /// </summary>
        /// <param name="valor">Nuevo valor de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void ForzarValor(object value, string codigoModuloLlamada, string descripcionLlamada)
        {
            if (OVariablesManager.Iniciado)
            {
                if (!this.Remoto)
                {
                    this.VariableCore.ForzarValor(value, codigoModuloLlamada, descripcionLlamada);
                }
                else
                {
                    // Se lanza desde un thread distino.
                    CambiaValorEnThread cambioValorEnThread = new CambiaValorEnThread(this.VariableCore.ForzarValor);
                    cambioValorEnThread.BeginInvoke(value, codigoModuloLlamada, descripcionLlamada, null, null);
                }
            }
        }

        /// <summary>
        /// Devuelve el tipo de la variable
        /// </summary>
        /// <returns></returns>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public OEnumTipoDato GetTipo()
        {
            if (OVariablesManager.Iniciado)
            {
                return this.VariableCore.GetTipo();
            }
            return OEnumTipoDato.SinDefinir;
        }

        /// <summary>
        /// Método que ejecuta los suscriptores
        /// </summary>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void Disparo(string codigoModuloLlamada, string descripcionLlamada)
        {
            if (OVariablesManager.Iniciado)
            {
                this.VariableCore.Disparo(codigoModuloLlamada, descripcionLlamada);
            }
        }

        /// <summary>
        /// Crea la suscripción a la variable
        /// </summary>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        /// <param name="delegadoSuscriptor">Delegado que queremos suscribir</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void CrearSuscripcion(string codigoModuloLlamada, string descripcionLlamada, OCambioValorDelegate delegadoSuscriptor)
        {
            if (OVariablesManager.Iniciado)
            {
                this.CambioValor += delegadoSuscriptor;
            }
        }

        /// <summary>
        /// Crea la suscripción a la variable
        /// </summary>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        /// <param name="delegadoSuscriptor">Delegado que queremos suscribir</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void CrearSuscripcion(string codigoModuloLlamada, string descripcionLlamada, OCambioValorDelegateAdvanced delegadoSuscriptor)
        {
            if (OVariablesManager.Iniciado)
            {
                this.CambioValorAvanzado += delegadoSuscriptor;
            }
        }

        /// <summary>
        /// Elimina la suscripción a la variable
        /// </summary>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        /// <param name="delegadoSuscriptor">Delegado que queremos desuscribir</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void EliminarSuscripcion(string codigoModuloLlamada, string descripcionLlamada, OCambioValorDelegate delegadoSuscriptor)
        {
            if (OVariablesManager.Iniciado)
            {
                this.CambioValor -= delegadoSuscriptor;
            }
        }

        /// <summary>
        /// Elimina la suscripción a la variable
        /// </summary>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        /// <param name="delegadoSuscriptor">Delegado que queremos desuscribir</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void EliminarSuscripcion(string codigoModuloLlamada, string descripcionLlamada, OCambioValorDelegateAdvanced delegadoSuscriptor)
        {
            if (OVariablesManager.Iniciado)
            {
                this.CambioValorAvanzado -= delegadoSuscriptor;
            }
        }

        #endregion

        #region Definición de delegado(s)
        /// <summary>
        /// Implementación del delegado que se activa cuando cambia el valor de la variable
        /// </summary>
        private event OCambioValorDelegate CambioValor = null;

        /// <summary>
        /// Implementación del delegado que se activa cuando cambia el valor de la variable
        /// </summary>
        private event OCambioValorDelegateAdvanced CambioValorAvanzado = null;

        #endregion

        #region Eventos
        /// <summary>
        /// Se llama a la ejecución de los delegados
        /// </summary>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        private void EjecutaEventos(OCambioValorEventArgs e)
        {
            try
            {
                if (!OThreadManager.EjecucionEnTrheadPrincipal())
                {
                    OThreadManager.SincronizarConThreadPrincipal(new CambioOValorEvent(EjecutaEventos), new object[] { e });
                    return;
                }

                if (this.CambioValor != null)
                {
                    // Ejecución del delegado
                    this.CambioValor();
                }
                if (this.CambioValorAvanzado != null)
                {
                    // Ejecución del delegado avanzado
                    this.CambioValorAvanzado(this.Codigo, e.Valor);
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosControl.Variables, "CambioValor", exception);
            }
        }

        /// <summary>
        /// Delegado usado para ejecutar un cambio de valor desde un thread
        /// </summary>
        /// <param name="valor">Nuevo valor de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        private delegate void CambiaValorEnThread(object value, string codigoModuloLlamada, string descripcionLlamada);
        #endregion
    }

    /// <summary>
    /// Variable de tipo genérico que se utiliza para que los diferenes módulos del sistema 
    /// se suscriban y reciban eventos cada vez que se modifique su valor.
    /// Separada de variable item para poder se accedida de forma remota.
    /// </summary>
    [Serializable]
    internal class VariableCore : MarshalByRefObject
    {
        #region Atributo(s)
        /// <summary>
        /// Crea y controla un subproceso, establece su prioridad y obtiene su estado.
        /// </summary>
        private Thread HiloEjecucionDelegadoRemoto;

        /// <summary>
        ///  Lista de remitentes que ya han consultado el cambio de valor de la variable.
        ///  Cada vez que un nuevo remitente consulta si la variable ha cambiado se almacena en esta lista 
        ///   para devolverle true únicamente la primera vez que consulta.
        ///  Cada vez que hay un cambio de valor de la variable se limpia la lista.
        /// </summary>
        private List<string> ListaConsultasCambioValor;
        #endregion

        #region Propiedad(es)

        /// <summary>
        /// Código identificativo de la variable
        /// </summary>
        private string Codigo;

        /// <summary>
        /// Valor de la variable
        /// </summary>
        private object Valor;

        /// <summary>
        /// Define si la variable puede modificarse o tiene fijado su valor
        /// </summary>
        private bool _Bloqueo;
        /// <summary>
        /// Define si la variable puede modificarse o tiene fijado su valor
        /// </summary>
        public bool Bloqueado
        {
            get { return _Bloqueo; }
            set { _Bloqueo = value; }
        }

        /// <summary>
        /// Define si la variable puede modificarse o tiene fijado su valor
        /// </summary>
        private bool _Inhibido;
        /// <summary>
        /// Define si la variable puede modificarse o tiene fijado su valor
        /// </summary>
        public bool Inhibido
        {
            get { return _Inhibido; }
            set { _Inhibido = value; }
        }

        /// <summary>
        /// Habilita o deshabilita el funcionamiento
        /// </summary>
        private bool _Habilitado;
        /// <summary>
        /// Habilita o deshabilita el funcionamiento
        /// </summary>
        public bool Habilitado
        {
            get { return _Habilitado; }
            set { _Habilitado = value; }

        }

        /// <summary>
        /// Indica que se ha de guardar la trazabilidad en la base de datos
        /// </summary>
        private bool _GuardarTrazabilidad;
        /// <summary>
        /// Indica que se ha de guardar la trazabilidad en la base de datos
        /// </summary>
        public bool GuardarTrazabilidad
        {
            get { return _GuardarTrazabilidad; }
            set { _GuardarTrazabilidad = value; }
        }

        /// <summary>
        /// Tipo de la variable
        /// </summary>
        private OEnumTipoDato _Tipo;
        /// <summary>
        /// Tipo de la variable
        /// </summary>
        public OEnumTipoDato Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }

        /// <summary>
        /// Cronometro del tiempo de permanencia del valor de la variable
        /// </summary>
        private Stopwatch _Cronometro;
        /// <summary>
        /// Cronometro del tiempo de permanencia del valor de la variable
        /// </summary>
        public Stopwatch Cronometro
        {
            get { return _Cronometro; }
            set { _Cronometro = value; }
        }
        #endregion

        #region Constructor(es)

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public VariableCore(string codigo, bool habilitado, OEnumTipoDato tipo, bool guardarTrazabilidad)
        {
            // Inicialiamos los valores
            this._Bloqueo = false;

            this.Codigo = codigo;
            this._Habilitado = habilitado;
            this._Tipo = tipo;
            this._GuardarTrazabilidad = guardarTrazabilidad;
            this._Cronometro = new Stopwatch();
            this.Cronometro.Reset();
            this.ListaConsultasCambioValor = new List<string>();

            this.HiloEjecucionDelegadoRemoto = new Thread(new ThreadStart(EjecutaDelegadoRemotoThread));
            this.HiloEjecucionDelegadoRemoto.IsBackground = true;
            this.HiloEjecucionDelegadoRemoto.Start();

            this.Valor = Orbita.VAComun.OTipoDato.DevaultValue(this.Tipo);
        }

        #endregion

        #region Método(s) privado(s)

        /// <summary>
        /// Se llama a la ejecución de los delegados
        /// </summary>
        [OneWay]
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        private void EjecutaDelegado()
        {
            if (!OThreadManager.EjecucionEnTrheadPrincipal())
            {
                OThreadManager.SincronizarConThreadPrincipal(new OSimpleMethod(EjecutaDelegado), new object[] { });
                return;
            }

            // Únicamente se llaman a los suscriptores cuando la variable está activa
            if (this._Habilitado)
            {
                // Ejecución del delegado local
                if (this.CambioValor != null)
                {
                    try
                    {
                            this.CambioValor(new OCambioValorEventArgs(this.Codigo, this.Valor));
                    }
                    catch (Exception exception)
                    {
                        OVALogsManager.Error(OModulosControl.Variables, "CambioValor", exception);
                    }
                }
            }
        }

        /// <summary>
        /// Se llama a la ejecución de los delegados
        /// </summary>
        [OneWay]
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        private void EjecutaDelegadoRemoto()
        {
            if (this._Habilitado)
            {
                lock (this)
                {
                    // Ejecución del delegado remoto de forma asincrona
                    Monitor.Pulse(this);
                }
            }
        }

        /// <summary>
        /// Guarda la información de la traza en la BBDD o en memoria
        /// </summary>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        /// <param name="tipoTraza">Tipo de evento que provocó la traza</param>
        private void NuevaTraza(string codigoModuloLlamada, string descripcionLlamada, TipoTraza tipoTraza)
        {
            // Trazabilidad en BBDD
            if (this._GuardarTrazabilidad)
            {
                OTrazaVariable trazaTrazabilidad = new OTrazaVariable(this.Codigo, this.Valor, tipoTraza, codigoModuloLlamada, descripcionLlamada);
                OVariablesManager.NuevaTraza(trazaTrazabilidad);
            }
        }

        /// <summary>
        /// Acciones a realizar tras establecer un determinado valor a la variable
        /// </summary>
        /// <param name="valor"></param>
        private void AccionesTrasEstablecerValor()
        {
                this.Cronometro.Reset();
                this.Cronometro.Start();
                lock (this.ListaConsultasCambioValor)
                {
                    this.ListaConsultasCambioValor.Clear();
                }

                // Se lanzan los suscriptores
                this.EjecutaDelegado();
                this.EjecutaDelegadoRemoto();
        }

        #endregion

        #region Método(s) público(s)

        /// <summary>
        /// Carga los valores de la variable
        /// </summary>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void Inicializar()
        {
            this.Cronometro.Start();
        }

        /// <summary>
        /// Finaliza la ejecución
        /// </summary>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void Finalizar()
        {
            this.Cronometro.Stop();
        }

        /// <summary>
        /// Lectura de la variable
        /// </summary>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public object GetValor()
        {
            return this.Valor;
        }

        /// <summary>
        /// Método para consultar el tiempo de permanencia del valor de la variable
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <returns>Devuelve el tiempo de permanencia del valor de la variable</returns>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public TimeSpan GetPermanencia()
        {
            return this.Cronometro.Elapsed;
        }

        /// <summary>
        /// Método para comprobar si el valor de la variable ha cambiado
        /// </summary>
        /// <returns>Devuelve verdadero si valor de la variable con el código correspondientes ha cambiado</returns>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        internal bool GetHayCambio(string codRemitente)
        {
            bool resultado = !this.ListaConsultasCambioValor.Exists(delegate(string cod) { return cod == codRemitente; });

            if (resultado)
            {
                lock (this.ListaConsultasCambioValor)
                {
                    this.ListaConsultasCambioValor.Add(codRemitente);
                }
            }

            return resultado;
        }

        /// <summary>
        /// Escritura de la variable
        /// </summary>
        /// <param name="valor">Nuevo valor de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void SetValor(object valor, string codigoModuloLlamada, string descripcionLlamada)
        {
            if ((this._Habilitado) && (this.Valor != valor) && (!this._Bloqueo) && (!this._Inhibido))
            {
                // Insertamos la traza
                this.NuevaTraza(codigoModuloLlamada, descripcionLlamada, TipoTraza.CambioValor);
                OVALogsManager.Debug(OModulosControl.Variables, "SetValor", "La variable " + this.Codigo + " cambia su valor a " + ORobusto.ToString(valor));

                // Establecimiento del valor
                this.Valor = valor;
                this.AccionesTrasEstablecerValor();
            }
        }

        /// <summary>
        /// Escritura de la variable de forma retrasada.
        /// Transcurido el tiempo especificado se modifica su valor al deseado
        /// </summary>
        /// <param name="valor">Nuevo valor de la variable</param>
        /// <param name="retraso">Tiempo de retraso de la actualización del valor</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void SetValorRetrasado(object valor, TimeSpan retraso, string codigoModuloLlamada, string descripcionLlamada)
        {
            if ((this._Habilitado) && (this.Valor != valor) && (!this._Bloqueo) && (!this._Inhibido))
            {
                // creo ua secuencia para el valor momentaneo
                OSecuencia secuencia = new OSecuencia(this.Codigo + "-Retrasado", System.Threading.ThreadPriority.BelowNormal, 1);
                secuencia.Add(new OSecuenciaItemValor(this.Codigo, valor, retraso));
                secuencia.Start();
            }
        }

        /// <summary>
        /// Método para modificar el valor de una variable a de forma registrada y bloquearla para que no se modifique
        /// </summary>
        /// <param name="valor">Nuevo valor de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void Bloquear(string codigoModuloLlamada, string descripcionLlamada)
        {
            if (this._Habilitado)
            {
                if (!this._Bloqueo)
                {
                    this._Bloqueo = true;

                    // Insertamos la traza
                    this.NuevaTraza(codigoModuloLlamada, descripcionLlamada, TipoTraza.Bloqueo);
                }
            }
        }

        /// <summary>
        /// Método para desbloquear una variable y que cualquiera pueda modificarla
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void Desbloquear(string codigoModuloLlamada, string descripcionLlamada)
        {
            if (this._Habilitado)
            {
                if (this._Bloqueo)
                {
                    this._Bloqueo = false;

                    // Insertamos la traza
                    this.NuevaTraza(codigoModuloLlamada, descripcionLlamada, TipoTraza.Desbloqueo);
                }
            }
        }

        /// <summary>
        /// Método para inhibir la modificación del valor de una variable
        /// </summary>
        /// <param name="valor">Nuevo valor de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void Inhibir(string codigoModuloLlamada, string descripcionLlamada)
        {
            if ((this._Habilitado) && (!this.Bloqueado))
            {
                if (!this._Inhibido)
                {
                    this._Inhibido = true;

                    // Insertamos la traza
                    this.NuevaTraza(codigoModuloLlamada, descripcionLlamada, TipoTraza.Inhibicion);
                }
            }
        }

        /// <summary>
        /// Método para desinhibir la modificación del valor de una variable
        /// </summary>
        /// <param name="codigo">Código de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void Desinhibir(string codigoModuloLlamada, string descripcionLlamada)
        {
            if ((this._Habilitado) && (!this.Bloqueado))
            {
                if (this._Inhibido)
                {
                    this._Inhibido = false;

                    // Insertamos la traza
                    this.NuevaTraza(codigoModuloLlamada, descripcionLlamada, TipoTraza.DesInhibicion);
                }
            }
        }

        /// <summary>
        /// Escritura de la variable cuando está bloqueada
        /// </summary>
        /// <param name="valor">Nuevo valor de la variable</param>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void ForzarValor(object valor, string codigoModuloLlamada, string descripcionLlamada)
        {
            if ((this._Habilitado) && (this.Valor != valor) && (this._Bloqueo))
            {
                // Insertamos la traza
                this.NuevaTraza(codigoModuloLlamada, descripcionLlamada, TipoTraza.ForzarValor);
                OVALogsManager.Info(OModulosControl.Variables, "SetValor", "La variable " + this.Codigo + " cambia su valor a " + ORobusto.ToString(valor));

                // Establecimiento del valor
                this.Valor = valor;
                this.AccionesTrasEstablecerValor();
            }
        }

        /// <summary>
        /// Devuelve el tipo de la variable
        /// </summary>
        /// <returns></returns>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public OEnumTipoDato GetTipo()
        {
            return this._Tipo;
        }

        /// <summary>
        /// Método que ejecuta los suscriptores
        /// </summary>
        /// <param name="codigoModuloLlamada">Código identificativo del módulo que modifica a la variable</param>
        /// <param name="descripcionLlamada">Descripción de la modificación de la variable</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void Disparo(string codigoModuloLlamada, string descripcionLlamada)
        {
            if (this._Habilitado)
            {
                // Insertamos la traza
                this.NuevaTraza(codigoModuloLlamada, descripcionLlamada, TipoTraza.DisparoSuscripciones);
                OVALogsManager.Debug(OModulosControl.Variables, "SetValor", "La variable " + this.Codigo + " realiza un disparo");

                // Establecimiento del valor
                this.AccionesTrasEstablecerValor();
            }
        }

        #endregion

        #region Método(s) en hilo
        /// <summary>
        /// Ejecuta los delegados remotos en un thread
        /// </summary>
        private void EjecutaDelegadoRemotoThread() 
        {
            while (true)
            {
                try
                {
                    lock (this)
                    {
                        // Espera a la activación del monitor para proceder con el thread
                        Monitor.Wait(this);
                        if (this.CambioValorRemoto != null)
                        {
                            this.CambioValorRemoto(new OCambioValorEventArgs(this.Codigo, this.Valor));
                        }
                    }
                }
                catch
                {
                }
            }
        }
        #endregion

        #region Definición de delegado(s)
        /// <summary>
        /// Implementación del evento que se activa cuando cambia el valor de la variable
        /// </summary>
        public event CambioOValorEvent CambioValor = null;

        /// <summary>
        /// Implementación del evento que se activa cuando cambia el valor de la variable de forma remota
        /// </summary>
        public event CambioOValorEvent CambioValorRemoto = null;
        #endregion
    }

    /// <summary>
    /// Clase utilizada para enlazar con los eventos del variablecore de forma remota
    /// </summary>
    [Serializable]
    public class OVariableBroadcastEventWraper : MarshalByRefObject
    {
        #region Evento(s)
        /// <summary>
        /// Evento de cambio de dato.
        /// </summary>
        public event CambioOValorEvent CambioValor;
        #endregion

        #region Método(s) público(s)

        /// <summary>
        /// Evento de cambio de dato.
        /// </summary>
        /// <param name="e">Argumento que puede ser utilizado en el manejador de evento.</param>
        [OneWay]
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public void OnCambioValor(OCambioValorEventArgs e)
        {
            try
            {
                if (CambioValor != null)
                {
                    // Se lanza desde un thread distino.                    
                    //this.CambioValor.BeginInvoke(e, null, null);  
                    this.CambioValor(e);
                }
            }
            catch{}
        }
        #endregion
    }

    /// <summary>
    /// Clase para acceder a los objetos VariableCore remotos
    /// </summary>
    [Serializable]
    internal class OGetRemoteVariableCore : MarshalByRefObject
    {
        #region Método(s) público(s)
        /// <summary>
        /// Método para la adquisición de la VariableCore por remoting
        /// </summary>
        /// <param name="codigo">Código de la varible</param>
        /// <returns>Objeto de tipo VariableCore</returns>
        public VariableCore GetVariableCore(string codigo)
        {
            OVariable variable;
            if (OVariablesManager.ListaVariables.TryGetValue(codigo, out variable))
            {
                return variable.VariableCore;
            }

            return null;
        }
        #endregion
    }

    #region Definición de delegado(s)
    /// <summary>
    /// Evento que se activa cuando cambia el valor de la variable
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void CambioOValorEvent(OCambioValorEventArgs e);

    /// <summary>
    /// Argumentos del evento
    /// </summary>
    [Serializable]
    public class OCambioValorEventArgs
    {
        #region Propiedad(es)
        /// <summary>
        /// Código identificativo de la variable
        /// </summary>
        private string _Codigo;
        /// <summary>
        /// Código identificativo de la variable
        /// </summary>
        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }

        /// <summary>
        /// Valor de la variable
        /// </summary>
        private object _Valor;
        /// <summary>
        /// Valor de la variable
        /// </summary>
        public object Valor
        {
            get { return _Valor; }
            set { _Valor = value; }
        }
        #endregion

        #region Constructor de la clase
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codigo">Código identificativo de la variable</param>
        /// <param name="valor">Valor de la variable</param>
        public OCambioValorEventArgs(string codigo, object valor)
        {
            this._Codigo = codigo;
            this._Valor = valor;
        }
        #endregion
    }

    /// <summary>
    /// Declaración del delegado que se activa cuando cambia el valor de una variable
    /// </summary>
    public delegate void OCambioValorDelegate();

    /// <summary>
    /// Declaración del delegado que se activa cuando cambia el valor de una variable
    /// </summary>
    public delegate void OCambioValorDelegateAdvanced(string codigo, object valor);

    #endregion

    #region Secuencias de ejecución
    /// <summary>
    /// Listado de secuencias.
    /// Se supone que al insertar cada uno de los items de la secuencia, estos tienen que estar ordenados cronológicamente
    /// </summary>
    public class OSecuencia : List<OSecuenciaItemBase>
    {
        #region Atributo(s)
        /// <summary>
        /// Código identificativo de la secuencia
        /// </summary>
        public string Codigo;

        /// <summary>
        /// Thread de ejecución
        /// </summary>
        OThread ThreadEjecucion;

        /// <summary>
        /// Momento en el que se inicia la ejecución de la secuencia
        /// </summary>
        private Stopwatch DuracionEjecucion;

        /// <summary>
        /// Número de repeticiones de la secuencia
        /// </summary>
        public int Repeticiones;

        /// <summary>
        /// Contador de repeticiones de la secuencia
        /// </summary>
        private uint ContRepeticiones;

        /// <summary>
        /// Contador de la secuencia actual
        /// </summary>
        private int ContSecuencias;

        /// <summary>
        /// Prioridad en la ejecución del thread
        /// </summary>
        private ThreadPriority ThreadPriority;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OSecuencia(string codigo, ThreadPriority threadPriority, int repeticiones)
            : base()
        {
            this.Codigo = codigo;
            this.Repeticiones = repeticiones;
            this.ContRepeticiones = 0;
            this.ContSecuencias = 0;
            this.ThreadPriority = threadPriority;

            this.DuracionEjecucion = new Stopwatch();

            this.ThreadEjecucion = new OThread("Secuencia_" + this.Codigo, 10, this.ThreadPriority);
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Ejecución dentro del thread
        /// </summary>
        private void ThreadRun(ref bool finalize)
        {
            finalize = true;
            try
            {
                if (this.Count > 0)
                {
                    if ((this.ContRepeticiones < this.Repeticiones) || (this.Repeticiones == -1))
                    {
                        if (this.ContSecuencias < this.Count)
                        {
                            OSecuenciaItemBase secuenciaItem = this[this.ContSecuencias];
                            TimeSpan momentoEjecucion = secuenciaItem.MomentoEjecucion;
                            if (this.DuracionEjecucion.Elapsed >= momentoEjecucion)
                            {
                                OThreadManager.SincronizarConThreadPrincipal(secuenciaItem.EjecutaElementoSecuencia, new object[] { });
                                this.ContSecuencias++;
                            }
                        }
                        else
                        {
                            this.ContRepeticiones++;
                            this.ContSecuencias = 0;
                            this.DuracionEjecucion.Reset();
                            this.DuracionEjecucion.Start();
                        }
                        finalize = false;
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosControl.Variables, "Secuencia", exception);
            }
        }

        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Inicia la secuencia
        /// </summary>
        public void Start()
        {
            this.DuracionEjecucion.Reset();
            this.DuracionEjecucion.Start();
            this.ContRepeticiones = 0;
            this.ContSecuencias = 0;

            //this.ThreadEjecucion.OnEjecucion += ThreadRun;
            this.ThreadEjecucion.CrearSuscripcionRun(ThreadRun, true);
            this.ThreadEjecucion.Start();
        }

        /// <summary>
        /// Finaliza la secuencia
        /// </summary>
        public void Stop()
        {
            this.DuracionEjecucion.Stop();

            //this.ThreadEjecucion.OnEjecucion -= ThreadRun;
            this.ThreadEjecucion.Stop(100);
        }

        /// <summary>
        /// Pasua la secuencia
        /// </summary>
        public void Pause()
        {
            this.DuracionEjecucion.Stop();
            this.ThreadEjecucion.Pause();
        }

        /// <summary>
        /// Continua la secuencia
        /// </summary>
        public void Resume()
        {
            this.DuracionEjecucion.Start();
            this.ThreadEjecucion.Resume();
        }
        #endregion
    }

    /// <summary>
    /// Clase base para los elementos de la secuencia
    /// </summary>
    public class OSecuenciaItemValor : OSecuenciaItemBase
    {
        #region Atributo(s)
        /// <summary>
        /// Valor a establecer en la variable
        /// </summary>
        public object Valor;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="valor">Valor a establecer en la variable</param>
        /// <param name="momentoEjecucion"></param>
        public OSecuenciaItemValor(string codVariable, object valor, TimeSpan momentoEjecucion)
            : base(codVariable, momentoEjecucion)
        {
            this.Valor = valor;
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="valor">Valor a establecer en la variable</param>
        /// <param name="momentoMinimoEjecucionAleatoria">Momento mínimo de la ejecución aleatoria</param>
        /// <param name="momentoMaximoEjecucionAleatoria">Momento máximio de la ejecución aleatoria</param>
        public OSecuenciaItemValor(string codVariable, object valor, TimeSpan momentoMinimoEjecucionAleatorio, TimeSpan momentoMaximoEjecucionAleatorio)
            : base(codVariable, momentoMinimoEjecucionAleatorio, momentoMaximoEjecucionAleatorio)
        {
            this.Valor = valor;
        }

        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Evento de ejecución del elemento de la secuencia
        /// </summary>
        /// <param name="codVariable"></param>
        /// <param name="valor"></param>
        /// <param name="esComando"></param>
        protected override void OnEjecuta()
        {
            OVariablesManager.SetValue(this.CodVariable, this.Valor, "Secuencia", this.CodVariable);
        }
        #endregion
    }

    /// <summary>
    /// Clase base para los elementos de la secuencia
    /// </summary>
    public class OSecuenciaItemComando : OSecuenciaItemBase
    {
        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="valor">Valor a establecer en la variable</param>
        /// <param name="momentoEjecucion"></param>
        public OSecuenciaItemComando(string codVariable, TimeSpan momentoEjecucion)
            : base(codVariable, momentoEjecucion)
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="valor">Valor a establecer en la variable</param>
        /// <param name="momentoMinimoEjecucionAleatoria">Momento mínimo de la ejecución aleatoria</param>
        /// <param name="momentoMaximoEjecucionAleatoria">Momento máximio de la ejecución aleatoria</param>
        public OSecuenciaItemComando(string codVariable, TimeSpan momentoMinimoEjecucionAleatorio, TimeSpan momentoMaximoEjecucionAleatorio)
            : base(codVariable, momentoMinimoEjecucionAleatorio, momentoMaximoEjecucionAleatorio)
        {
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Evento de ejecución del elemento de la secuencia
        /// </summary>
        /// <param name="codVariable"></param>
        /// <param name="valor"></param>
        /// <param name="esComando"></param>
        protected override void OnEjecuta()
        {
            OVariablesManager.Dispara(this.CodVariable, "Secuencia", this.CodVariable);
        }
        #endregion
    }

    /// <summary>
    /// Clase base para los elementos de la secuencia
    /// </summary>
    public class OSecuenciaItemBase
    {
        #region Atributo(s)
        /// <summary>
        /// Código de la variable sobre la que se aplica el valor
        /// </summary>
        public string CodVariable;

        /// <summary>
        /// Indica que el momento de ejecución se realiza de forma aleatoria
        /// </summary>
        public bool MomentoEjecucionAleatorio;

        /// <summary>
        /// Momento más bajo de la ejecución aleatoria
        /// </summary>
        public TimeSpan MomentoMinimoEjecucionAleatorio;

        /// <summary>
        /// Momento más alto de la ejecución aleatoria
        /// </summary>
        public TimeSpan MomentoMaximoEjecucionAleatorio;

        /// <summary>
        /// Indica que la secuencia ya ha sido ejecutada
        /// </summary>
        public bool Ejecutado;

        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Momento de ejecución respecto al inicio de la secuencia
        /// </summary>
        private TimeSpan _MomentoEjecucion;
        /// <summary>
        /// Momento de ejecución respecto al inicio de la secuencia
        /// </summary>
        public TimeSpan MomentoEjecucion
        {
            get
            {
                if (this.MomentoEjecucionAleatorio)
                {
                    Random r = new Random();
                    double miliseconds = r.Next(Convert.ToInt32(this.MomentoMinimoEjecucionAleatorio.TotalMilliseconds), Convert.ToInt32(this.MomentoMaximoEjecucionAleatorio.TotalMilliseconds));
                    this._MomentoEjecucion = TimeSpan.FromMilliseconds(miliseconds);
                }
                return _MomentoEjecucion;
            }
            set { _MomentoEjecucion = value; }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="momentoEjecucion"></param>
        public OSecuenciaItemBase(string codVariable, TimeSpan momentoEjecucion)
        {
            this.CodVariable = codVariable;
            this.MomentoEjecucion = momentoEjecucion;
            this.MomentoEjecucionAleatorio = false;
            this.EjecutaElementoSecuencia += OnEjecuta;
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="momentoMinimoEjecucionAleatoria">Momento mínimo de la ejecución aleatoria</param>
        /// <param name="momentoMaximoEjecucionAleatoria">Momento máximio de la ejecución aleatoria</param>
        public OSecuenciaItemBase(string codVariable, TimeSpan momentoMinimoEjecucionAleatorio, TimeSpan momentoMaximoEjecucionAleatorio)
        {
            this.CodVariable = codVariable;

            this.MomentoEjecucionAleatorio = true;
            this.MomentoMinimoEjecucionAleatorio = momentoMinimoEjecucionAleatorio;
            this.MomentoMaximoEjecucionAleatorio = momentoMaximoEjecucionAleatorio;
            this.EjecutaElementoSecuencia += OnEjecuta;
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Evento de ejecución del elemento de la secuencia
        /// </summary>
        /// <param name="codVariable"></param>
        /// <param name="valor"></param>
        /// <param name="esComando"></param>
        protected virtual void OnEjecuta()
        {
        }
        #endregion

        #region Definición de delegado(s)
        /// <summary>
        /// Delegado que de la ejecución de un elemento de la secuencia
        /// </summary>
        internal delegate void DelegadoEjecutaElementoSecuencia();

        /// <summary>
        /// Delegado para la ejecución sincronizada
        /// </summary>
        internal DelegadoEjecutaElementoSecuencia EjecutaElementoSecuencia;
        #endregion
    }

    #endregion

    #region Trazabilidad de variables
    internal class OTrazabilidadVariables
    {
        #region Atributo(s)

        /// <summary>
        /// Cola de las trazas
        /// </summary>
        private Queue<OTrazaVariable> ColaTrazas;
        /// <summary>
        /// Timer de guardado de las trazas
        /// </summary>
        private System.Windows.Forms.Timer TimerEjecucion;

        #endregion

        #region Constructor
        public OTrazabilidadVariables()
        {
            this.ColaTrazas = new Queue<OTrazaVariable>();
        }
        #endregion

        #region Método(s) público(s)

        /// <summary>
        /// Carga las propiedades de la base de datos
        /// </summary>
        public void Inicializar()
        {
            // Parametrización del timer
            this.TimerEjecucion = new System.Windows.Forms.Timer();
            this.TimerEjecucion.Interval = Convert.ToInt32(Math.Round(OVariablesManager.TiempoPermanenciaTrazasEnMemoria.TotalMilliseconds));
            this.TimerEjecucion.Tick += new EventHandler(EventoTimerEjecucion);
            this.TimerEjecucion.Start();
        }

        /// <summary>
        /// Se para la ejecución
        /// </summary>
        public void Finalizar()
        {
            // Finalización del timer
            this.TimerEjecucion.Stop();
            this.TimerEjecucion.Tick -= new EventHandler(EventoTimerEjecucion);
            this.TimerEjecucion = null;
        }

        /// <summary>
        /// Añade una nueva traza a la cola
        /// </summary>
        /// <param name="traza"></param>
        public void NuevaTraza(OTrazaVariable traza)
        {
            lock (this.ColaTrazas)
            {
                this.ColaTrazas.Enqueue(traza);
            }

            // Guardamos la traza de registros
            string info = string.Empty;
            info += "Modulo: " + traza.CodModuloLlamada + "; ";
            info += "Descripción: " + traza.DescripcionLlamada + "; ";
            switch (traza.TipoTraza)
            {
                case TipoTraza.CambioValor:
                    info += "Tipo: Cambio de valor; ";
                    break;
                case TipoTraza.DisparoSuscripciones:
                    info += "Tipo: Disparo suscripciones; ";
                    break;
            }
            if (traza.Valor != null)
            {
                info += "Valor: " + ORobusto.ToString(traza.Valor) + "; ";
            }
            else
            {
                info += "Valor: (vacio); ";
            }
            OVALogsManager.Debug(OModulosControl.Variables, traza.CodVariable, info);
        }

        #endregion

        #region Método(s) privado(s)

        /// <summary>
        /// Extrae las trazas que han de ser almacenadas en la base de datos
        /// </summary>
        /// <returns>Cola con las trazas a almacenar en la BBDD.</returns>
        private Queue<OTrazaVariable> ObtenerTrazasAGuardar()
        {
            Queue<OTrazaVariable> colaTrazas = new Queue<OTrazaVariable>();

            bool salirBucle = false;
            while (!salirBucle)
            {
                salirBucle = (this.ColaTrazas.Count == 0);

                if (!salirBucle)
                {
                    OTrazaVariable traza = this.ColaTrazas.Peek();

                    salirBucle = (traza.Fecha > DateTime.Now - OVariablesManager.TiempoPermanenciaTrazasEnMemoria);

                    if (!salirBucle)
                    {
                        OVariable variable = OVariablesManager.GetVariable(traza.CodVariable);
                        salirBucle = (variable == null) || (!variable.GuardarTrazabilidad);
                    }

                    if (!salirBucle)
                    {
                        lock (this.ColaTrazas)
                        {
                            colaTrazas.Enqueue(this.ColaTrazas.Dequeue());
                        }
                    }
                }
            }

            return colaTrazas;
        }

        /// <summary>
        /// Formatea las trazas que necesiten ser guardadas en un XML
        /// </summary>
        /// <returns>Objeto de la clase CXML con los datos formateados.</returns>
        private OCXML ConvertirTrazasXML(Queue<OTrazaVariable> colaTrazas)
        {
            OCXML oXML = new OCXML();
            while (colaTrazas.Count > 0)
            {
                OTrazaVariable traza = colaTrazas.Dequeue();

                oXML.AbrirEtiqueta("TrazaAdd");
                oXML.Add("CodVariable", traza.CodVariable);
                oXML.Add("CodModulo", traza.CodModuloLlamada);
                oXML.Add("Descripcion", traza.DescripcionLlamada);
                if (traza.TipoTraza == TipoTraza.CambioValor)
                {
                    oXML.Add("Valor", ORobusto.ToString(traza.Valor));
                }
                else
                {
                    oXML.Add("Valor", string.Empty);
                }
                oXML.Add("Tipo", (int)traza.TipoTraza);
                oXML.Add("Fecha", traza.Fecha.ToString("dd/MM/yyyy HH:mm:ss.FFF")); // Es necesario utilizar este formato para añadir decimales a los segundos
                oXML.CerrarEtiqueta();
            }

            oXML.CerrarDocumento();

            return oXML;
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento del timer de ejecución
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void EventoTimerEjecucion(object sender, EventArgs e)
        {
            try
            {
                if (this.ColaTrazas.Count > 0)
                {
                    Queue<OTrazaVariable> colaTrazas = this.ObtenerTrazasAGuardar();

                    if (colaTrazas.Count > 0)
                    {
                        OCXML oXML = this.ConvertirTrazasXML(colaTrazas);

                        int ret = AppBD.AddTrazas(oXML);

                        if (ret != 0)
                        {
                            throw new Exception("Ha sido imposible enviar información de trazabilidad de variables a la base de datos.");
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosControl.Variables, "Trazabilidad", exception);
            }
        }

        #endregion
    }

    internal struct OTrazaVariable
    {
        #region Atributo(s)
        public string CodVariable;
        public string CodModuloLlamada;
        public string DescripcionLlamada;
        public object Valor;
        public TipoTraza TipoTraza;
        public DateTime Fecha;
        #endregion

        #region Constructor
        public OTrazaVariable(string codVariable, object valor, TipoTraza tipoTraza, string codModuloLlamada, string descripcionLlamada)
        {
            this.CodVariable = codVariable;
            this.Valor = valor;
            this.TipoTraza = tipoTraza;
            this.CodModuloLlamada = codModuloLlamada;
            this.DescripcionLlamada = descripcionLlamada;
            this.Fecha = DateTime.Now;
        }
        #endregion
    }

    /// <summary>
    /// Indica el origen de la inserción de la traza
    /// </summary>
    internal enum TipoTraza
    {
        CambioValor = 1,
        DisparoSuscripciones = 2,
        Bloqueo = 3,
        Desbloqueo = 4,
        ForzarValor = 5,
        Inhibicion = 6,
        DesInhibicion = 7
    }
    #endregion
}
