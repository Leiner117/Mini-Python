# Minics.exe

Cuatro módulos principales:

1) Módulo CODIGO: permite almacenar las instrucciones de bajo nivel del lenguaje que va a ejecutar la máquina virtual. 
Cada instrucción debe contemplar su nombre, y su parámetro en caso de que tenga. Debe contemplar métodos para alimentar
la estructura de instrucciones a partir de un buffer de entrada.

2) Módulo PILA: permite almacenar valores en una pila para eventualmente ser usados como parte de las operaciones de pila 
de la máquina virtual. Debe considerar principalmente usar dos tipos de elementos a sabr enteros y chars

3) Módulo ALMACEN: permite almacenar tuplas de variables y valores (enteros y chars) que serán utilizados por el intérprete para
resolver las operaciones de busqueda y almacenamiento de identificadores. Así mismo debe almacenar referencias a métodos que 
deben llevar la "dirección" en el código que tiene dicho método.

4) Módulo INSTR-SET: donde se implemengarán los métodos para implementar/ejecutar cada instrucción del lenguaje. Harán uso de los 
tres módulos anteriores

Adicional a esto, se implementarán las clases que corran todo el interprete.