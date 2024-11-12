using System;
using System.Collections;

namespace moduloPila
{
    public class Pila
    {

        Stack pila = new Stack ();

        public void push (dynamic obj){
            pila.Push(obj);
        }

        public dynamic pop(){
            return pila.Pop ();
        }

       public void imprimir () {
        foreach ( Object obj in pila) {
        Console.Write( "    {0}", obj );
        }
       }
            
    }
}
