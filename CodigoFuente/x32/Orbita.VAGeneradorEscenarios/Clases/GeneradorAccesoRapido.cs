//***********************************************************************
// Assembly         : Orbita.VAGeneradorEscenarios
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;

namespace CodeGenerator
{
    /// <summary>
    /// Clase encargada de escribir el código C# para permitir el acceso rápido a variables.
    /// Lee de la tabla de vistas y crea una clase para cada una de ellas
    /// </summary>
    class GeneradorAccesoRapido
    {
        #region Campos
        /// <summary>
        /// Unit de compilación
        /// </summary>
        public CodeCompileUnit CodeCompileUnit;

        /// <summary>
        /// Espacio de nombres
        /// </summary>
        public CodeNamespace NameSpace;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public GeneradorAccesoRapido()
        {
            // Create a new CodeCompileUnit to contain the program graph.
            this.CodeCompileUnit = new CodeCompileUnit();            
        }
        #endregion    

        #region Métodos públicos
        /// <summary>
        /// Genera la unit
        /// </summary>
        public void GenerarUnit(string espacioNombres, params string[] importaciones)
        {
            // Declare a namespace
            this.NameSpace = new CodeNamespace(espacioNombres);

            // Add the new namespace to the compile unit.
            this.CodeCompileUnit.Namespaces.Add(this.NameSpace);

            // Add the new namespace import for the System namespace.
            foreach (string importacion in importaciones)
            {
                this.NameSpace.Imports.Add(new CodeNamespaceImport(importacion));
            }
        }

        /// <summary>
        /// Genera la clase
        /// </summary>
        public CodeTypeDeclaration GenerarClase(string nombre, string padre, string comentarios, MemberAttributes atributos, bool partial)
        {
            CodeTypeDeclaration clase = null;

            bool existe = false;
            foreach (CodeTypeDeclaration claseEnEspacioNombres in this.NameSpace.Types)
            {
                existe = claseEnEspacioNombres.Name == nombre;
                if (existe)
                {
                    break;
                }
            }

            // Declare a new class
            if (!existe)
            {
                clase = new CodeTypeDeclaration(nombre);
                clase.IsClass = true;
                clase.Comments.Add(new CodeCommentStatement(comentarios));
                clase.Attributes = atributos;
                if (padre != string.Empty)
                {
                    clase.BaseTypes.Add(padre);
                }

                clase.IsPartial = partial;

                // Add the new type to the namespace type collection.
                this.NameSpace.Types.Add(clase);
            }

            return clase;
        }

        /// <summary>
        /// Genera la clase dentro de otra clase
        /// </summary>
        public CodeTypeDeclaration GenerarClase(string nombre, string padre, CodeTypeDeclaration claseContinente, string comentarios, MemberAttributes atributos)
        {
            CodeTypeDeclaration clase = null;

            bool existe = false;
            foreach (CodeTypeMember claseEnClase in claseContinente.Members)
            {
                existe = (claseEnClase.Name == nombre);
                if (existe)
                {
                    break;
                }
            }

            // Declare a new class
            if (!existe)
            {
                clase = new CodeTypeDeclaration(nombre);
                clase.IsClass = true;
                clase.Comments.Add(new CodeCommentStatement(comentarios));
                clase.Attributes = atributos;
                if (padre != string.Empty)
                {
                    clase.BaseTypes.Add(padre);
                }

                // Add the new type to the namespace type collection.
                if (!claseContinente.Members.Contains(clase))
                {
                    claseContinente.Members.Add(clase);
                }
            }

            return clase;
        }

        /// <summary>
        /// Genera un método
        /// </summary>
        public CodeMemberMethod GenerarMetodo(string nombre, CodeTypeDeclaration claseContinente, string comentarios, MemberAttributes atributos, CodeParameterDeclarationExpression[] parametros, CodeStatement[] instrucciones)
        {
            // Declare a new class
            CodeMemberMethod metodo = new CodeMemberMethod();
            metodo.Attributes = atributos;
            metodo.Comments.Add(new CodeCommentStatement(comentarios));
            metodo.Name = nombre;
            metodo.Parameters.AddRange(parametros);

            metodo.Statements.AddRange(instrucciones);

            // Add the new type to the namespace type collection.
            claseContinente.Members.Add(metodo);

            return metodo;
        }

        /// <summary>
        /// Instrucción de asignación
        /// </summary>
        /// <returns></returns>
        public CodeStatement Asignacion(string left, string right)
        {
            return new CodeAssignStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(), left),
                    new CodeVariableReferenceExpression(right));
        }

        /// <summary>
        /// Instrucción de creación de objeto
        /// </summary>
        /// <returns></returns>
        public CodeStatement New(string objeto, string tipo, string parametro)
        {
            return new CodeAssignStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(), objeto),
                    new CodeObjectCreateExpression(tipo, new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(), parametro)));
        }

        /// <summary>
        /// Genera un constructor
        /// </summary>
        public CodeConstructor GenerarConstructor(CodeTypeDeclaration claseContinente, string comentarios, MemberAttributes atributos, CodeParameterDeclarationExpression[] parametros, CodeExpression[] parametrosBase, CodeStatement[] instrucciones)
        {
            // Declare a new class
            CodeConstructor metodo = new CodeConstructor();
            metodo.Attributes = atributos;
            metodo.Comments.Add(new CodeCommentStatement(comentarios));

            metodo.Parameters.AddRange(parametros);
            metodo.BaseConstructorArgs.AddRange(parametrosBase);

            metodo.Statements.AddRange(instrucciones);

            // Add the new type to the namespace type collection.
            claseContinente.Members.Add(metodo);

            return metodo;
        }

        /// <summary>
        /// Genera la propiedad
        /// </summary>
        public void GenerarPropiedadReadOnly(ref CodeTypeDeclaration clase, string nombre, string cast, string comentarios, MemberAttributes atributos, string tipo, bool generarCampo)
        {
            string nombreCampo = "_" + nombre;
            if (generarCampo)
            {
                // Añadir campo
                CodeMemberField campo = new CodeMemberField(tipo, nombreCampo);
                campo.Attributes = MemberAttributes.Private;
                campo.Comments.Add(new CodeCommentStatement(comentarios));
                clase.Members.Add(campo);
            }

            // Llamada al campo
            CodeExpression expresion;
            CodeFieldReferenceExpression expresionLlamadaCampo = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), nombreCampo);
            if (cast != string.Empty)
            {
                expresion = new CodeCastExpression(cast, expresionLlamadaCampo);
            }
            else
            {
                expresion = expresionLlamadaCampo;
            }

            // Propiedad
            CodeMemberProperty varProperty = new CodeMemberProperty();
            varProperty.Attributes = atributos;
            varProperty.Name = nombre;
            varProperty.HasGet = true;
            varProperty.Type = new CodeTypeReference(tipo);
            varProperty.Comments.Add(new CodeCommentStatement(comentarios));
            varProperty.GetStatements.Add(new CodeMethodReturnStatement(expresion));

            clase.Members.Add(varProperty);
        }

        /// <summary>
        /// Genera la propiedad
        /// </summary>
        public void GenerarPropiedadReadOnlyRuntime(ref CodeTypeDeclaration clase, string nombre, string cast, string comentarios, MemberAttributes atributos, string tipo, string referenciaTipo, string referenciaMetodo, params CodeExpression[] referenciaParametros)
        {
            CodeExpression expresion;

            // Llamada al método
            CodeMethodInvokeExpression methodExpresion = new CodeMethodInvokeExpression(
                new CodeMethodReferenceExpression(
                new CodeTypeReferenceExpression(referenciaTipo), referenciaMetodo));
            foreach (CodeExpression parametro in referenciaParametros)
            {
                methodExpresion.Parameters.Add(parametro);
            }

            if (cast != string.Empty)
            {
                expresion = new CodeCastExpression(cast, methodExpresion);
            }
            else
            {
                expresion = methodExpresion;
            }
            
            // Propiedad
            CodeMemberProperty varProperty = new CodeMemberProperty();
            varProperty.Attributes = atributos;
            varProperty.Name = nombre;
            varProperty.HasGet = true;
            varProperty.Type = new CodeTypeReference(tipo);
            varProperty.Comments.Add(new CodeCommentStatement(comentarios));
            varProperty.GetStatements.Add(new CodeMethodReturnStatement(expresion));

            clase.Members.Add(varProperty);
        }

        /// <summary>
        /// Genera la propiedad
        /// </summary>
        public void GenerarPropiedadRuntime(ref CodeTypeDeclaration clase, string nombre, string cast, string comentarios, MemberAttributes atributos, string tipo, string campo)
        {
            // Llamada al método GET
            CodeExpression expresionGET = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_" + nombre);
            expresionGET = new CodeCastExpression(cast, expresionGET);

            // Llamada al método SET
            CodeAssignStatement expresionSET = new CodeAssignStatement(
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), campo), 
                new CodePropertySetValueReferenceExpression());

            // Propiedad
            CodeMemberProperty varProperty = new CodeMemberProperty();
            varProperty.Attributes = atributos;
            varProperty.Name = nombre;
            varProperty.HasGet = true;
            varProperty.Type = new CodeTypeReference(tipo);
            varProperty.Comments.Add(new CodeCommentStatement(comentarios));
            varProperty.GetStatements.Add(new CodeMethodReturnStatement(expresionGET));
            varProperty.SetStatements.Add(expresionSET);

            clase.Members.Add(varProperty);
        }

        /// <summary>
        /// Guarda el codigo en el fichero
        /// </summary>
        /// <param name="destCodeFilePath"></param>
        public void GuardarCodigo(string destCodeFilePath)
        {
            // Abrir el fichero
            TextWriter writeFile = new StreamWriter(destCodeFilePath, false);

            // Escribir el código en el fichero
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            provider.GenerateCodeFromCompileUnit(this.CodeCompileUnit, writeFile, options);

            // Cerrar el fichero
            writeFile.Flush();
            writeFile.Close();
        }

        /// <summary>
        /// Añade una región alrededor de la clase
        /// </summary>
        /// <param name="codeRegionMode"></param>
        /// <param name="regionName"></param>
        /// <param name="clase"></param>
        public void Region(string regionName, CodeTypeDeclaration clase)
        {
            clase.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, regionName));
            clase.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, string.Empty));

            //CodeTypeDeclaration ctd = new CodeTypeDeclaration();
            //ctd.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, regionName));
            //ctd.Members.AddRange(new CodeTypeMember[1] { clase });
            //ctd.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, string.Empty));
        }

        /// <summary>
        /// Inicia una región al principio de la clase
        /// </summary>
        /// <param name="codeRegionMode"></param>
        /// <param name="regionName"></param>
        /// <param name="clase"></param>
        public void RegionStart(string regionName, CodeTypeDeclaration clase)
        {
            clase.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, regionName));
        }

        /// <summary>
        /// Finalzia una región al principio de la clase
        /// </summary>
        /// <param name="codeRegionMode"></param>
        /// <param name="regionName"></param>
        /// <param name="clase"></param>
        public void RegionEnd(CodeTypeDeclaration clase)
        {
            clase.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, string.Empty));
        }

        #endregion
    }
}
