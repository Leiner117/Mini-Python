namespace Mini_Python.compilador.Checker;
using System;
using System.Collections.Generic;
using Antlr4.Runtime;
public class TablaSimbolos {

        // Tabla de símbolos como una pila (Stack) para manejar scopes
        private LinkedList<Ident> tabla;
        private int nivelActual;

        // Clase abstracta que representa un símbolo en la tabla
        public abstract class Ident {
            public IToken Tok { get; }
            public SymbolType Type { get; }
            public int Nivel { get; }
            public int Valor { get; set; }

            public Ident(IToken tok, SymbolType type, int nivel) {
                Tok = tok;
                Type = type;
                Nivel = nivel;
                Valor = 0; // Valor por defecto
            }

            public abstract void PrintIdent(); // Método para imprimir detalles del símbolo
        }

        // Clase para representar variables en la tabla
        public class VarIdent : Ident {
            public bool IsConstant { get; }

            public VarIdent(IToken tok, SymbolType type, int nivel, bool isConstant)
                : base(tok, type, nivel) {
                IsConstant = isConstant;
            }

            public override void PrintIdent() {
                Console.WriteLine($"Variable: {Tok.Text}, Tipo: {Type}, Nivel: {Nivel}, Constante: {IsConstant}");
            }
        }

        // Clase para representar funciones en la tabla
        public class MethodIdent : Ident {
            public List<string> Params { get; }

            public MethodIdent(IToken tok, SymbolType type, int nivel, List<string> parameters)
                : base(tok, type, nivel) {
                Params = parameters;
            }

            public override void PrintIdent() {
                Console.WriteLine($"Funcion: {Tok.Text}, Tipo: {Type}, Nivel: {Nivel}, Parametros: {Params.Count}");
            }
        }

        // Constructor
        public TablaSimbolos() {
            tabla = new LinkedList<Ident>();
            nivelActual = -1; // Nivel inicial
        }

        // Métodos para abrir y cerrar scopes (alcances)
        public void OpenScope() {
            nivelActual++;
        }

        public void CloseScope() {
            tabla = new LinkedList<Ident>(
                tabla.Where(ident => ((Ident)ident).Nivel != nivelActual)
            );
            nivelActual--;
        }

        // Insertar una variable en la tabla de símbolos
        public void InsertarVariable(IToken id, SymbolType tipo) {
            VarIdent varIdent = new VarIdent(id, tipo, nivelActual, false);
            tabla.AddFirst(varIdent);
        }

        // Insertar una función en la tabla de símbolos
        public void InsertarFuncion(IToken id, SymbolType tipo, List<string> paramsList) {
            Ident methodIdent = new MethodIdent(id, tipo,nivelActual, paramsList);
            tabla.AddFirst(methodIdent);
        }

        // Buscar un símbolo en la tabla de símbolos (todos los niveles)
        public Ident Buscar(string nombre) {
            foreach (var id in tabla) {
                if (id.Tok.Text.Equals(nombre))
                    return id;
            }
            return null;
        }

        // Buscar un símbolo solo en el nivel actual
        public Ident BuscarEnNivelActual(string nombre) {
            foreach (var id in tabla) {
                if (id.Nivel == nivelActual && id.Tok.Text.Equals(nombre))
                    return id;
            }
            return null;
        }

        // Imprimir la tabla de símbolos
        public void Imprimir() {
            Console.WriteLine("----- INICIO TABLA ------");

            // Recorremos la lista de la tabla de símbolos
            foreach (var id in tabla)
            {
                if (id is VarIdent varIdent)
                {
                    Console.WriteLine($"Variable: {varIdent.Tok.Text}, Nivel: {varIdent.Nivel}, Tipo: {varIdent.Type}, Constante: {varIdent.IsConstant}");
                }
                else if (id is MethodIdent methodIdent)
                {
                    Console.WriteLine($"Funcion: {methodIdent.Tok.Text}, Nivel: {methodIdent.Nivel}, Tipo: {methodIdent.Type}, Parametros: {string.Join(",", methodIdent.Params)}");
                }
            }

            Console.WriteLine("----- FIN TABLA ------");
        }
    }

    // Enumeración para representar los tipos de símbolos (Variable, Function, etc.)
    public enum SymbolType {
        Variable,
        Function,
        Parameter
    }
