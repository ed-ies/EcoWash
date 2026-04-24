En este ejemplo vamos a hacer uso de diferentes test unitarios que nos permitan probar las diferentes funcionalidades de los principales Commands de nuestra aplicación.

# 1. PanelTiendasViewModelTest
En primer lugar debemos de añadir un proyecto de testing a nuestra solución. Para ello, vamos a nuestra solución y añadimos un nuevo proyecto de "Proyecto de Prueba de MSTest".

Una vez incluido, tenemos que añadir a nuestro proyecto de prueba el proyecto EcoWash como referencia, para ello nos vamos a **Agregar** -> **Referencia de proyecto** -> **EcoWash**.

Hecho esto, ya podemos comenzar a realizar nuestras pruebas unitarias sobre los diferentes comandos. Lo normal es hacer una clase de test por cada clase que queramos probar. En este ejemplo vamos a testear la clase **PanelTiendasViewModel.cs**, para ello vamos a crear una nueva clase en nuestro proyecto de pruebas que vamos a llamar **PanelTiendasViewModelTest.cs**. En ella vamos a testear el único método que tenemos que ese ViewModel, el cual es **CargarCommand**. 

`PanelTiendasViewModelTest.cs`
```csharp
using EcoWash.ViewModels;

namespace TestProject1
{
    [TestClass]
    public class PanelTiendasViewModelTests
    {
        // Testeamos el comando de CargarCommand
        [TestMethod]
        public async Task Test_CargarCommand()
        {
            var vm = new PanelTiendasViewModel();

            vm.CargarCommand.Execute(null);

            await Task.Delay(2000);

            Assert.IsTrue(vm.ListaTiendasVisibles.Count > 0);
        }
    }
}
```

# 2. TiendasCrudViewModelTest
Lo siguiente sería testear que el ViewModel del CRUD funciona adecuadamente, para ello vamos a crear una nueva clase que nos permita testear cada uno de los commands que tenemos en ese ViewModel. Vamos a crear una nueva clase de pruebas que se llame **TiendasCrudViewModelTest.cs** y vamos a dar de alta los diferentes test ahí.

`TiendasCrudViewModelTest.cs`
```csharp
using EcoWash.ViewModels;

namespace TestProject1
{
    [TestClass]
    public class TiendasCrudViewModelTests
    {
    
    }
}
```
## 2.1 CargarComand

En este test vamos a comprobar que los usuarios de la base de datos de prueba se cargan adecuadamente.
```csharp
// Testeamos el comando de CargarCommand
[TestMethod]
public async Task Test_CargarCommand()
{
    var vm = new TiendasCrudViewModel();

    vm.CargarCommand.Execute(null);

    await Task.Delay(2000);

    Assert.IsTrue(vm.ListaTiendasVisibles.Count > 0);
}
```

# 2.2 NuevaTiendaCommand

Ahora vamos a comprobar que se cree una tienda nueva en memoria de forma correcta, comprobando que los datos por defecto son los que hemos programado.
```csharp
// Testeamos el comando de NuevaTiendaCommand
// Creamos una nueva tienda y comprobamos que los datos existan en memoria
[TestMethod]
public void Test_NuevaTiendaCommand()
{
    var vm = new TiendasCrudViewModel();

    vm.NuevaTiendaCommand.Execute(null);

    Assert.AreEqual(0, vm.Id);
    Assert.AreEqual(1, vm.Lavadoras.Count);
    Assert.IsFalse(vm.Lavadoras[0].IsOccupied);
}
```

## 2.3 NuevaLavadoraCommand

Ahora vamos a añadir una nueva lavadora a una tienda, para ello no podemos tener en cuenta la tienda que creamos antes, ya que las pruebas unitarias muchas veces se ejecutan en paralelo, por lo que cada una de ellas ha de preparar sus propios datos de prueba y no deben de depender nunca la una de la otra.
```csharp
// Testeamos el comando de NuevaTiendaCommand
// Creamos una nueva tienda ficticia y le añadimos una lavadora, si el conteo llega a 2 , se ha hecho correctamente
[TestMethod]
public void Test_NuevaLavadoraCommand()
{
    var vm = new TiendasCrudViewModel();
    vm.NuevaTiendaCommand.Execute(null);

    vm.NuevaLavadoraCommand.Execute(null);

    Assert.AreEqual(2, vm.Lavadoras.Count);
}
```

## 2.4 BorrarLavadoraCommand

En este test vamos a comprobar que las lavadoras se eliminan adecuadamente, además, que se respeta la norma de que cuando solo queda una de ellas, esta no es eliminada.

```csharp
// Testeamos el comando de BorrarLavadoraCommand
// Creamos una tienda con su lavadora y la intentamos eliminar, como siempre ha de haber una, la lavadora debe de seguir ahi
[TestMethod]
public void Test_BorrarLavadoraCommand()
{
    var vm = new TiendasCrudViewModel();
    vm.NuevaTiendaCommand.Execute(null);
    vm.NuevaLavadoraCommand.Execute(null);

    // Eliminamos la primera lavadora
    var lavadoraAEliminar = vm.Lavadoras.First();
    vm.BorrarLavadoraCommand.Execute(lavadoraAEliminar);

    // Intentamos eliminar la segunda lavadora
    lavadoraAEliminar = vm.Lavadoras.First();
    vm.BorrarLavadoraCommand.Execute(lavadoraAEliminar);


    Assert.AreEqual(1, vm.Lavadoras.Count);
}
```

## 2.5 GuardarTiendaCommand

En este test vamos a guardar una nueva tienda en la base de datos, para ello vamos a crear una tienda por defecto y la vamos a guardar, una vez guardada vamos a comprobar que se lista adecuadamente. Además, una vez acabado el test, tenemos que borrar los datos generados para dejar la base de datos tal cual antes de los tests.

```csharp
// Testeamos el comando GuardarTiendaCommand pero en su version de Insertar
// Vamos a guardar una nueva tienda en la BBDD
[TestMethod]
public async Task Test_GuardarTienda_Nueva()
{
    // Creamos la tienda
    var vm = new TiendasCrudViewModel();

    vm.NuevaTiendaCommand.Execute(null);
    vm.Nombre = "TIENDA_TEST_CRUD";
    vm.Direccion = "Direccion Test";
    vm.Potencia = 12.5;

    vm.GuardarTiendaCommand.Execute(null);
    await Task.Delay(2000);

    Assert.IsTrue(vm.Id > 0);

    // Limpiamos los datos que hemos usado de test
    vm.BorrarTiendaCommand.Execute(null);
    await Task.Delay(2000);
}
```

## 2.6 GuardarTiendaCommand

En esta prueba vamos a volver a testear el mismo comando pero esta vez guardando los cambios realizados a una tienda ya existente en la base de datos, de esta manera probaremos el comando de actualizar.

```csharp
// Testeamos el comando GuardarTiendaCommand pero en su version de Actualizar
// Vamos a actualizar una tienda ya existente en la BBDD y comprobar sus cambios
[TestMethod]
public async Task Test_GuardarTienda_Editar()
{
    // Creamos una tienda de ejemplo
    var vm = new TiendasCrudViewModel();
    int _tiendaIdCreada;

    vm.NuevaTiendaCommand.Execute(null);
    vm.Nombre = "TIENDA_TEST_CRUD";
    vm.Direccion = "Direccion Test";
    vm.Potencia = 12.5;

    vm.GuardarTiendaCommand.Execute(null);
    await Task.Delay(2000);
    _tiendaIdCreada = vm.Id;

    // Editamos la tienda
    var tienda = vm.ListaTiendasVisibles
        .First(t => t.Id == _tiendaIdCreada);

    vm.Selected = tienda;
    vm.SelectedItemChangedCommand.Execute(null);

    vm.Nombre = "TIENDA_TEST_EDITADA";

    vm.GuardarTiendaCommand.Execute(null);
    await Task.Delay(2000);

    // Recargar y comprobar
    vm.CargarCommand.Execute(null);
    await Task.Delay(1500);

    var tiendaEditada = vm.ListaTiendasVisibles
        .First(t => t.Id == _tiendaIdCreada);

    Assert.AreEqual("TIENDA_TEST_EDITADA", tiendaEditada.Nombre);

    // Limpiamos los datos que hemos usado de test
    vm.BorrarTiendaCommand.Execute(null);
    await Task.Delay(2000);
}
```

## 2.7 BorrarTiendaCommand

Finalmente vamos a probar el comando de borrar la tienda, creando y guardando una de prueba en la base de datos y luego borrándola mediante su ID y comprobando que realmente ha sido borrada.

```csharp
// Testeamos el comando BorrarTiendaCommand
// Vamos a comprobar que las tiendas se eliminan adecuadamente y no queda rastro de ellas en la BBDD
[TestMethod]
public async Task Test_BorrarTiendaCommand()
{
    // Creamos una tienda de ejemplo
    var vm = new TiendasCrudViewModel();
    int _tiendaIdCreada;

    vm.NuevaTiendaCommand.Execute(null);
    vm.Nombre = "TIENDA_TEST_CRUD";
    vm.Direccion = "Direccion Test";
    vm.Potencia = 12.5;

    vm.GuardarTiendaCommand.Execute(null);
    await Task.Delay(2000);
    _tiendaIdCreada = vm.Id;

    // Borramos la tienda creada
    var tienda = vm.ListaTiendasVisibles
        .First(t => t.Id == _tiendaIdCreada);

    vm.Selected = tienda;
    vm.SelectedItemChangedCommand.Execute(null);

    vm.BorrarTiendaCommand.Execute(null);
    await Task.Delay(2000);

    vm.CargarCommand.Execute(null);
    await Task.Delay(1500);

    Assert.IsFalse(vm.ListaTiendasVisibles.Any(t => t.Id == _tiendaIdCreada));
}
```
