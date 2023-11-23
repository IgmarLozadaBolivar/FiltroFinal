<h1 align="center"><img width="48" height="48" src="https://img.icons8.com/fluency/48/garden.png" alt="Safe Clothing"/><b>Garden's App - DbFirst</b></h1>

<p>La empresa Gardens especializada en Jardineria desea construir una aplicacion que le permita llevar el control y registro de todos sus productos y servicios. Empleando el DbFirst, trabajando la estructura de un proyecto de 4 capas y logrando una optima respuesta de migraciones habilitadas, consultas y demas!</p>

## Tecnologias 🧑🏻‍💻
<p align="center">
<img src="https://user-images.githubusercontent.com/73097560/115834477-dbab4500-a447-11eb-908a-139a6edaec5c.gif"><br>

- **Back-End Development**: 
  ![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=flat&logo=c-sharp&logoColor=white) 
  ![.Net](https://img.shields.io/badge/.NET-5C2D91?style=flat&logo=.net&logoColor=white) 

- **Softwares & Tools**: 
  ![Visual Studio Code](https://img.shields.io/badge/Visual%20Studio%20Code-0078d7.svg?style=flat&logo=visual-studio-code&logoColor=white) 
  ![Postman](https://img.shields.io/badge/Postman-FF6C37?style=flat&logo=postman&logoColor=white) 
  ![Swagger](https://img.shields.io/badge/-Swagger-%23Clojure?style=flat&logo=swagger&logoColor=white) 
  ![Insomnia](https://img.shields.io/badge/Insomnia-black?style=flat&logo=insomnia&logoColor=5849BE) 
  ![GIT](https://img.shields.io/badge/Git-fc6d26?style=flat&logo=git&logoColor=white)

- **Database**:
  ![MySQL](https://img.shields.io/badge/mysql-%2300f.svg?style=flat&logo=mysql&logoColor=white)

</p>

<img src="https://user-images.githubusercontent.com/73097560/115834477-dbab4500-a447-11eb-908a-139a6edaec5c.gif"><br>

## Requerimientos funcionales 👻<br>
🎯 Generar el Proyecto Backend usando DBFirst o CodeFirst. ✔ <br>
🎯 Organizar el Proyecto usando 4 Capas. ✔ <br>
🎯 Genere reppositorio para el Proyecto. El enlace del repositorio debe ser enviado por medio del correo electronico
<a href="Johlverpardo.campuslands@gmail.com">Johlverpardo.campuslands@gmail.com</a>. ✔ <br>
<details>
  <summary>Estructura de envio</summary>
    Asunto : Enlace filtro Nombres_Apellidos y Grupo. <br>
    Cuerpo : Nombres, Apellidos y Grupo <br>
</details>
<br>
🎯 Se debe generar el README del proyecto teniendo en cuenta las siguientes especificaciones. ✔ <br>
<details>
  <summary>Especificaciones</summary>
    Enunciado de la consulta <br>
    EndPoint de la Consulta <br>
    Codigo de la consulta <br>
    Explicacion de la consulta <br>
</details>
<br>
🎯 Fecha de entrega : 23 Noviembre al finalizar el Skill. ✔ <br>
🎯 Tiempo estimado de la prueba : 4 hras. ✔ <br>

<img src="https://user-images.githubusercontent.com/73097560/115834477-dbab4500-a447-11eb-908a-139a6edaec5c.gif"><br>

## Conteo de Consultas realizadas: `Total 10/30` ✅ <br>

<img src="https://user-images.githubusercontent.com/73097560/115834477-dbab4500-a447-11eb-908a-139a6edaec5c.gif"><br>

## Consultas 👨‍💻 <br>
**METODO**: `GET`

**🔰 ENUNCIADO 1: Devuelve un listado con el código de pedido, código de cliente, fecha esperada y fecha de entrega de los pedidos que `no han sido entregados a tiempo` ✅**

**ENDPOINT**: `http://localhost:5124/api/Pedido/PedidosQueNoFueronEntregadosATiempo`

**CODIGO DE CONSULTA + LINQ**:
```sql
    public async Task<IEnumerable<object>> PedidosQueNoFueronEntregadosATiempo()
    {
        var mensaje = "Pedidos que no fueron entregados a tiempo".ToUpper();

        var consulta = from p in _context.Pedidos
                       where p.FechaEsperada < p.FechaEntrega
                       select new
                       {
                           CodigoDePedido = p.CodigoPedido,
                           CodigoDeCliente = p.CodigoCliente,
                           FechaEsperada = p.FechaEsperada,
                           FechaEntrega = p.FechaEntrega,
                       };

        var resultado = await consulta.ToListAsync();

        var resultadoFinal = new List<object>
        {
            new { Title = mensaje, DatosConsultados = resultado }
        };

        return resultadoFinal;
    }
```
**EXPLICACION**:
Accedemos al contexto de la entidad de Pedido, en la cual se le pasa como condicion que la fecha de entrega sea mayor a la fecha esperada, de un pedido ya realizado, con el objetivo de retornar un objeto que contiene los pedidos cuyos que no han sido entregados a tiempo y un pequeño mensaje.

**METODO**: `GET`

**🔰 ENUNCIADO 2: Devuelve el nombre de los clientes que no hayan hecho pagos y el nombre de sus representantes junto con la ciudad de la oficina a la que pertenece el representante ✅** 

**ENDPOINT**: `http://localhost:5124/api/Cliente/ClientesQueNoHayanHechoPagos`

**CODIGO DE CONSULTA + LINQ**:

```sql
    public async Task<IEnumerable<object>> ClientesQueNoHayanHechoPagos()
    {
        var mensaje = "Listado de clientes que no hicieron pagos y se retorna, el nombre del cliente, el nombre y ciudad de la oficina del representante".ToUpper();

        var consulta = from c in _context.Clientes
                       join p in _context.Pagos on c.CodigoCliente equals p.CodigoCliente into pagos
                       join e in _context.Empleados on c.CodigoEmpleadoRepVentas equals e.CodigoEmpleado
                       join o in _context.Oficinas on e.CodigoOficina equals o.CodigoOficina
                       from pago in pagos.DefaultIfEmpty()
                       where pago == null
                       select new
                       {
                           NombreDelCliente = c.NombreCliente,
                           NombreDelRepresentante = e.Nombre,
                           Apellido1DelRepresentante = e.Apellido1,
                           Apellido2DelRepresentante = e.Apellido2,
                           CiudadDeLaOficinaDelRepresentante = o.Ciudad
                       };

        var resultado = await consulta.ToListAsync();

        var resultadoFinal = new List<object>
        {
            new { Msg = mensaje, DatosConsultados = resultado}
        };

        return resultadoFinal;
    }
```
**EXPLICACION**:
Accedemos al contexto de la entidad de Cliente, en la cual se le pasa como condicion que en un cliente que no haya realizado pago alguno, con el objetivo de retornar un objeto que contiene el nombre del cliente, nombre, apellidos y ciudad de la oficina de dicho representante.

**METODO**: `GET`

**🔰 ENUNCIADO 3: Devuelve las oficinas donde no trabajan ninguno de los empleados que hayan sido los representantes de ventas de algún cliente que haya realizado la compra de algún producto de la gama Frutales ✅**

**ENDPOINT**: `http://localhost:5124/api/Oficina/OficinasDondeNoTrabajanNingunEmpleadoEnGamaFrutal`

**CODIGO DE CONSULTA + LINQ**:
```sql
    public async Task<IEnumerable<object>> OficinasDondeNoTrabajaNingunEmpleadoEnGamaFrutal()
    {
        var mensaje = "Oficinas donde no trabaja ningun empleado en la gama frutal".ToUpper();

        var consulta = await _context.Oficinas
            .GroupJoin(
                _context.Empleados,
                o => o.CodigoOficina,
                e => e.CodigoOficina,
                (o, empleadosGroup) => new { Oficina = o, Empleados = empleadosGroup }
            )
            .SelectMany(
                x => x.Empleados.DefaultIfEmpty(),
                (oficina, empleado) => new { Oficina = oficina.Oficina, Empleado = empleado }
            )
            .Where(x => x.Empleado == null || !_context.Clientes.Any(c =>
                        c.CodigoEmpleadoRepVentas == x.Empleado.CodigoEmpleado &&
                        _context.Pedidos.Any(pd =>
                            pd.CodigoCliente == c.CodigoCliente &&
                            _context.DetallePedidos.Any(dp =>
                                dp.CodigoPedido == pd.CodigoPedido &&
                                _context.Productos.Any(p =>
                                    dp.CodigoProducto == p.CodigoProducto &&
                                    p.Gama == "Frutales"
                                )
                            )
                        )
                    )
            )
            .Select(x => new
            {
                CodigoOficina = x.Oficina.CodigoOficina,
                Ciudad = x.Oficina.Ciudad,
                Region = x.Oficina.Region,
                Pais = x.Oficina.Pais,
                CodigoPostal = x.Oficina.CodigoPostal,
                Telefono = x.Oficina.Telefono,
                LineasDeDireccion = $"{x.Oficina.LineaDireccion1}, {x.Oficina.LineaDireccion2}"
            })
            .ToListAsync();

        var resultadoFinal = new List<object>
        {
            new { Msg = mensaje, DatosConsultados = consulta }
        };

        return resultadoFinal;
    }
```
**EXPLICACION**:
Accedemos al contexto de las entidades de `Oficina` y `Empleados`, en la cual al condicicion es que se verifique si una oficina no cuenta con un empleado, se realiza una agrupacion de datos, y condiciones como la gama perteneciente, la validacion de clientes anexados, y retornando datos de la oficina que no cuenta con un empleado en la gama frutal.

**METODO**: `GET`

**🔰 ENUNCIADO 4: Devuelve un listado de los 20 productos más vendidos y el número total de unidades que se han vendido de cada uno. El listado deberá estar ordenado por el número total de unidades vendidas ✅** 

**ENDPOINT**: `http://localhost:5124/api/DetallePedido/20ProductosMasVendidos`

**CODIGO DE CONSULTA + LINQ**:
```sql
    public async Task<IEnumerable<object>> VeinteProductosMasVendidos()
    {
        var mensaje = "20 productos mas vendidos y el numero total de ventas de cada uno".ToUpper();

        var consulta = await _context.DetallePedidos
            .GroupBy(detallePedido => detallePedido.CodigoProducto)
            .Select(grp => new
            {
                CodigoDeProducto = grp.Key,
                TotalUnidadesVendidas = grp.Sum(dp => dp.Cantidad)
            })
            .OrderByDescending(result => result.TotalUnidadesVendidas)
            .Take(20)
            .ToListAsync();

        var resultadoFinal = new List<object>
        {
            new { Title = mensaje, DatosConsultados = consulta }
        };

        return resultadoFinal;
    }
```
**EXPLICACION**:
Accedemos al contexto de las entidades de `DetallePedido`, en la cual se realiza una agrupacion por codigo de producto, sumando las cantidades que existan, ordenandolos de manera descendente, un maximo de 20 productos imprimidos y retornando un objeto con los datos consultados y un pequeño mensaje.

**METODO**: `GET`

**🔰 ENUNCIADO 5: Lista las ventas totales de los productos que hayan facturado más de 3000 euros. Se mostrará el nombre, unidades vendidas, total facturado y total facturado con impuestos (21% IVA) ✅** 

**ENDPOINT**: `http://localhost:5124/api/DetallePedido/ProductosQueFacturaronMasDe3000Euros`

**CODIGO DE CONSULTA + LINQ**:
```sql
    public async Task<object> ProductosQueFacturaronMasDe3000Euros()
    {
        var mensaje = "Productos que facturaron mas de 3000 euros".ToUpper();

        var grupos = await _context.DetallePedidos
        .GroupBy(detallePedido => detallePedido.CodigoProducto)
        .ToListAsync();

        var consulta = grupos
            .Join(
                _context.Productos,
                grupo => grupo.Key,
                producto => producto.CodigoProducto,
                (grupo, producto) => new
                {
                    NombreProducto = producto.Nombre,
                    UnidadesVendidas = grupo.Sum(dp => dp.Cantidad),
                    TotalFacturado = grupo.Sum(dp => dp.Cantidad * dp.PrecioUnidad),
                    TotalFacturadoConIVA = grupo.Sum(dp => dp.Cantidad * dp.PrecioUnidad * (decimal)1.21)
                })
            .Where(resultado => resultado.TotalFacturadoConIVA > 3000)
            .OrderByDescending(resultado => resultado.TotalFacturadoConIVA)
            .ToList();

        var resultadoFinal = new List<object>
        {
            new { Title = mensaje, DatosConsultados = consulta }
        };

        return resultadoFinal;
    }
```
**EXPLICACION**:
Accedemos al contexto de las entidades de `Productos`, en la cual, se realiza varios Join, en la cual se suma la cantidad de unidades vendidas de dicho producto, los productos se agrupan por codigo de producto, se realiza la respectiva operacion para obtener el facturado sin IVA y con IVA como resultado esperado, y se retorna un objeto.

**METODO**: `GET`

**🔰 ENUNCIADO 6: Devuelve el nombre del producto del que se han vendido más unidades. (Tenga en cuenta que tendrá que calcular cuál es el número total de unidades que se han vendido de cada producto a partir de los datos de la tabla detalle_pedido) ✅**

**ENDPOINT**: `http://localhost:5124/api/DetallePedido/ProductoQueVendioMasUnidades`

**CODIGO DE CONSULTA + LINQ**:
```sql
    public async Task<object> ProductoQueVendioMasUnidades()
    {
        var mensaje = "Producto que vendio mas unidades".ToUpper();

        var productoMasVendido = await _context.DetallePedidos
        .GroupBy(detallePedido => detallePedido.CodigoProducto)
        .OrderByDescending(grupo => grupo.Sum(dp => dp.Cantidad))
        .Select(grupo => grupo.Key)
        .FirstOrDefaultAsync();

        var nombreProductoMasVendido = await _context.Productos
            .Where(producto => producto.CodigoProducto == productoMasVendido)
            .Select(producto => producto.Nombre)
            .FirstOrDefaultAsync();

        var resultado = new List<object>
        {
            new { Title = mensaje, DatosConsultados = nombreProductoMasVendido}
        };

        return resultado;
    }
```
**EXPLICACION**:
Accedemos al contexto de las entidades de `DetallePedido` y `Producto`, en la cual, se realiza una agrupacion a partir del codigo del producto, se realiza una suma y se imprime de manera descendente, segun la cantidad que se encuentre registrada, y se retorna un objeto.

**METODO**: `GET`

**🔰 ENUNCIADO 7: Devuelve el listado de clientes indicando el nombre del cliente y cuántos pedidos ha realizado. Tenga en cuenta que pueden existir clientes que no han realizado ningún pedido ✅**

**ENDPOINT**: `http://localhost:5124/api/Cliente/ClientesYCantidadDePedidosRealizados`

**CODIGO DE CONSULTA + LINQ**:
```sql
    public async Task<object> ClientesYCantidadDePedidosRealizados()
    {
        var mensaje = "Retornar clientes y cantidad de pedidos realizados".ToUpper();

        var clientesConPedidos = await _context.Clientes
        .Select(cliente => new
        {
            cliente.NombreCliente,
            PedidosRealizados = _context.Pedidos.Count(pedido => pedido.CodigoCliente == cliente.CodigoCliente)
        })
        .ToListAsync();

        var resultado = new List<object>
        {
            new { Title = mensaje, DatosConsultados = clientesConPedidos }
        };

        return resultado;
    }
```
**EXPLICACION**:
Accedemos al contexto de las entidades de `Cliente`, Se realiza una seleccion, un conteo de pedidos por cada cliente que haya realizado uno, y se retorna un objeto.


**METODO**: `GET`

**🔰 ENUNCIADO 8: Devuelve el listado de clientes donde aparezca el nombre del cliente, el nombre y primer apellido de su representante de ventas y la ciudad donde está su oficina ✅**

**ENDPOINT**: `http://localhost:5124/api/Producto/ClientesNombresRepresentantesOficinas`

**CODIGO DE CONSULTA + LINQ**:
```sql
    public async Task<object> ClientesNombresRepresentantesOficinas()
    {
        var clientesConRepresentanteYOficina = await _context.Clientes
            .Where(cliente => cliente.CodigoEmpleadoRepVentas != null)
            .Select(cliente => new
            {
                NombreCliente = cliente.NombreCliente,
                NombreRepresentante = cliente.CodigoEmpleadoRepVentasNavigation.Nombre,
                ApellidoRepresentante = cliente.CodigoEmpleadoRepVentasNavigation.Apellido1,
                CiudadOficinaRepresentante = cliente.CodigoEmpleadoRepVentasNavigation.CodigoOficinaNavigation.Ciudad
            })
            .ToListAsync();

        return clientesConRepresentanteYOficina;
    }
```

**EXPLICACION**:
Accedemos al contexto de las entidades de `Empleado`, `Oficina`,  y `Cliente`, 
se retorna un objeto de varios datos de distintas entidades.

**METODO**: `GET`

**🔰 ENUNCIADO 9: Listado de empleados que no tienen un cliente y se retorna el nombre de su jefe ✅**

**ENDPOINT**: `http://localhost:5124/api/Producto/ProductosQueNoHanAparecidoEnUnPedido`

**CODIGO DE CONSULTA + LINQ**:
```sql
    public async Task<IEnumerable<object>> EmpleadosQueNoTieneUnCliente()
    {
        var mensaje = "Listado de empleados que no tienen un cliente y se retorna el nombre de su jefe".ToUpper();

        var empleadosSinClientesNiOficinas = await (
            from empleado in _context.Empleados
            join cliente in _context.Clientes on empleado.CodigoEmpleado equals cliente.CodigoCliente into clientes
            from cliente in clientes.DefaultIfEmpty()
            where cliente == null
            join jefe in _context.Empleados on empleado.CodigoJefe equals jefe.CodigoEmpleado into jefes
            from jefe in jefes.DefaultIfEmpty()
            select new
            {
                NombreDelEmpleado = empleado.Nombre,
                ApellidosDelEmpleado = $"{empleado.Apellido1}, {empleado.Apellido2}",
                NombreDelJefe = (jefe != null) ? jefe.Nombre : "Sin Jefe"
            })
            .ToListAsync();

        var resultadoFinal = new List<object>
        {
            new { Msg = mensaje, DatosConsultados = empleadosSinClientesNiOficinas }
        };

        return resultadoFinal;
    }
```

**EXPLICACION**:
Accedemos al contexto de las entidades de `Empleado` y `Cliente`, se realiza una condicion, en la cual se verifica si el empleado tiene clientes, y se retorna un objeto.

**METODO**: `GET`

**🔰 ENUNCIADO 10: Devuelve un listado de los productos que nunca han aparecido en un pedido ✅**

**ENDPOINT**: `http://localhost:5124/api/Producto/ProductosQueNoHanAparecidoEnUnPedido`

**CODIGO DE CONSULTA + LINQ**:
```sql
    public async Task<IEnumerable<object>> ProductosQueNoHanAparecidoEnUnPedido()
    {
        var mensaje = "Productos que no han aparecido en un pedido".ToUpper();

        var consulta = from pr in _context.Productos
                       join dp in _context.DetallePedidos
                       on pr.CodigoProducto equals dp.CodigoProducto into gj
                       from subpedido in gj.DefaultIfEmpty()
                       where subpedido == null
                       select new
                       {
                           CodigoProducto = pr.CodigoProducto,
                           NombreProducto = pr.Nombre,
                           GamaProducto = pr.Gama,
                           Dimensiones = pr.Dimensiones,
                           Proveedores = pr.Proveedor,
                           Descripcion = pr.Descripcion,
                           CantidadEnStock = pr.CantidadEnStock,
                           PrecioVenta = pr.PrecioVenta,
                           PrecioProveedor = pr.PrecioProveedor
                       };

        var resultadoFinal = new List<object>
    {
        new { Msg = mensaje, DatosConsultados = await consulta.ToListAsync() }
    };

        return resultadoFinal;
    }
```

<img src="https://user-images.githubusercontent.com/73097560/115834477-dbab4500-a447-11eb-908a-139a6edaec5c.gif"><br>

## Authors and collaborators:
- Powered by <a href="https://github.com/IgmarLozadaBolivar">Igmar Lozada</a><br>
- Collaborating with <a href="training.leader2023@gmail.com"> training.leader2023@gmail.com</a>

<img src="https://user-images.githubusercontent.com/73097560/115834477-dbab4500-a447-11eb-908a-139a6edaec5c.gif"><br>

## Thank you for reading this documentation and that you have observed this interesting project!

<img src="https://user-images.githubusercontent.com/73097560/115834477-dbab4500-a447-11eb-908a-139a6edaec5c.gif"><br>
