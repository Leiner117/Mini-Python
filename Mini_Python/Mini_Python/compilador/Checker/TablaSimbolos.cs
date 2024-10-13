namespace Mini_Python.compilador.Checker;
using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using System.Linq;

public class TablaSimbolos {

    // Tabla de símbolos como una lista de diccionarios para manejar scopes
    private List<Dictionary<string, Ident>> tabla;
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
        tabla = new List<Dictionary<string, Ident>>();
        nivelActual = -1; // Nivel inicial
    }

    // Métodos para abrir y cerrar scopes (alcances)
    public void OpenScope() {
        nivelActual++;
        if (nivelActual >= tabla.Count) {
            tabla.Add(new Dictionary<string, Ident>());
        }
        //tabla.Add(new Dictionary<string, Ident>());
    }

    public void CloseScope() {
        if (nivelActual >= 0) {
            tabla[nivelActual].Clear();
            nivelActual--;
        }
    }

    // Insertar una variable en la tabla de símbolos
    public void InsertarVariable(IToken id, SymbolType tipo)
    {
        if (nivelActual >= 0 && nivelActual < tabla.Count)
        {
            VarIdent varIdent = new VarIdent(id, tipo, nivelActual, false);
            tabla[nivelActual][id.Text] = varIdent;
        }
    }

    // Insertar una función en la tabla de símbolos
    public void InsertarFuncion(IToken id, SymbolType tipo, List<string> paramsList)
    {
        if (nivelActual >= 0 && nivelActual < tabla.Count)
        {
            Ident methodIdent = new MethodIdent(id, tipo, nivelActual, paramsList);
            tabla[nivelActual][id.Text] = methodIdent;
        }
    }

    // Buscar un símbolo en la tabla de símbolos (todos los niveles)
    public Ident Buscar(string nombre)
    {
        for (int i = nivelActual; i >= 0; i--)
        {
            if (tabla[i].ContainsKey(nombre))
            {
                return tabla[i][nombre];
            }
        }
        return null;
    }

    // Buscar un símbolo solo en el nivel actual
    public Ident BuscarEnNivelActual(string nombre)
    {
        if (nivelActual >= 0 && nivelActual < tabla.Count && tabla[nivelActual].ContainsKey(nombre))
        {
            return tabla[nivelActual][nombre];
        }
        return null;
    }

    // Imprimir la tabla de símbolos
    public void Imprimir()
    {
        Console.WriteLine("----- INICIO TABLA ------");
        for (int i = 0; i <= nivelActual; i++)
        {
            Console.WriteLine($"Nivel {i}:");
            foreach (var id in tabla[i].Values)
            {
                id.PrintIdent();
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
