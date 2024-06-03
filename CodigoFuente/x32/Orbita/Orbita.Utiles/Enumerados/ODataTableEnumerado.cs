//***********************************************************************
// Assembly         : OrbitaUtiles
// Author           : vnicolau
// Created          : 09-03-2011
//
// Last Modified By : crodriguez
// Last Modified On : 05-04-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Data;
namespace Orbita.Utiles
{
    /// <summary>
    /// Clase para obtener datatables con los valores de un enumerado
    /// </summary>
    public class ODataTableEnumerado
    {
        #region Método(s) público(s)
        /// <summary>
        /// Obtiene un datatable con los diferentes valores que puede tomar un enumerado.
        /// Devuelve la información en dos columnas:
        ///  - Identificador: corresponde al valor entero que el compilador o el programador le asigna a cada enumerado
        ///  - Valor: el ToString() de cada enumerado
        /// </summary>
        /// <param name="enumerado">Enumerado del cual se quiere extraer la información.</param>
        /// <returns>Devuelve la información en dos columnas:
        ///  - Identificador: corresponde al valor entero que el compilador o el programador le asigna a cada enumerado
        ///  - Valor: el ToString() de cada enumerado</returns>
        public static DataTable GetValoresDataTable(Type enumerado)
        {
            return GetValoresDataTable(enumerado, false);
        }
        /// <summary>
        /// Obtiene un datatable con los diferentes valores que puede tomar un enumerado.
        /// Devuelve la información en dos columnas:
        ///  - Identificador: corresponde al valor entero que el compilador o el programador le asigna a cada enumerado
        ///  - Valor: en función del segundo parámetro, devuelve el ToString() de cada enumerado o bien el atributo OAtributoEnumerado de cada enumerado
        /// </summary>
        /// <param name="enumerado">Enumerado del cual se quiere extraer la información.</param>
        /// <param name="obtenerValoresDelOAtributoEnumerado">True si se quiere obtener como Valor el atributo OAtributoEnumerado de cada enumerado; false si se quiere obteenr el ToString() de cada enumerado</param>
        /// <returns>Devuelve la información en dos columnas:
        ///  - Identificador: corresponde al valor entero que el compilador o el programador le asigna a cada enumerado
        ///  - Valor: en función del segundo parámetro, devuelve el ToString() de cada enumerado o bien el atributo OAtributoEnumerado de cada enumerado</returns>
        public static DataTable GetValoresDataTable(Type enumerado, bool obtenerValoresDelOAtributoEnumerado)
        {
            // Construir el datatable a devolver.
            DataTable dt = new DataTable();
            dt.Columns.Add("Identificador", typeof(System.Int32)); //El identificador corresponde al valor entero que el compilador o el programador le asigna a cada enumerado
            dt.Columns.Add("Valor", typeof(System.String));

            DataRow dr;
            Array valoresEnumerado = System.Enum.GetValues(enumerado);

            for (int i = 0; i < valoresEnumerado.Length; i++)
            {
                dr = dt.NewRow();
                dr["Identificador"] = Convert.ToInt32((Enum)valoresEnumerado.GetValue(i));
                if (obtenerValoresDelOAtributoEnumerado)
                {
                    if (OStringEnumerado.GetValorString((Enum)valoresEnumerado.GetValue(i)) != null
                        && OStringEnumerado.GetValorString((Enum)valoresEnumerado.GetValue(i)).Trim() != string.Empty)
                    {
                        dr["Valor"] = OStringEnumerado.GetValorString((Enum)valoresEnumerado.GetValue(i));
                    }
                    else
                    {
                        throw new System.ArgumentException("El enumerado " + valoresEnumerado.GetValue(i).ToString() + " no tiene definido ningún atributo de tipo OAtributoEnumerado, o éste está vacío.");
                    }
                }
                else
                {
                    dr["Valor"] = valoresEnumerado.GetValue(i).ToString();
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion
    }
}
