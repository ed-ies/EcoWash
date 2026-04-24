using EcoWash.Models;
using EcoWash.Services;
using EcoWash.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace EcoWash.ViewModels
{
    public class TiendasCrudViewModel : BaseViewModel
    {
        #region atributos
        // Coleccion para almacenar las tiendas
        private List<Tienda> _listaTiendas;
        private ObservableCollection<Tienda> _listaTiendasVisibles;

        // Declaramos un objeto de tipo Tienda para la tienda que manipulamos en el CRUD
        private Tienda _tienda;

        // Declaramos un objeto de tipo Tienda para la seleccionada
        private Tienda _selected;
        #endregion

        #region propiedades
        // ID de la tienda con la que trabajamos
        public int Id
        {
            get { return _tienda.Id; }
            set { _tienda.Id = value; OnPropertyChanged(); }
        }

        // NOMBRE de la tienda con la que trabajamos
        public string Nombre
        {
            get { return _tienda.Nombre; }
            set { _tienda.Nombre = value; OnPropertyChanged(); }
        }

        // DIRECCION de la tienda con la que trabajamos
        public string Direccion
        {
            get { return _tienda.Direccion; }
            set { _tienda.Direccion = value; OnPropertyChanged(); }
        }

        // IsACTIVE de la tienda con la que trabajamos
        public bool IsActive
        {
            get { return _tienda.IsActive; }
            set { _tienda.IsActive = value; OnPropertyChanged(); }
        }

        // POTENCIA de la tienda con la que trabajamos
        public double Potencia
        {
            get { return _tienda.Potencia; }
            set { _tienda.Potencia = value; OnPropertyChanged(); }
        }

        // Lista de Lavadoras de la tienda con la que trabajamos
        public ObservableCollection<Lavadora> Lavadoras
        {
            get { return (ObservableCollection<Lavadora>)_tienda.Lavadoras; }
            set { _tienda.Lavadoras = value; OnPropertyChanged(); }
        }

        // Lista de tiendas
        public ObservableCollection<Tienda> ListaTiendasVisibles
        {
            get => _listaTiendasVisibles;
            set => SetProperty(ref _listaTiendasVisibles, value);
        }

        // Tienda seleccionada
        public Tienda Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }
        #endregion

        #region constructor
        // Declaramos los diferentes ICommands
        public ICommand CargarCommand { get; }
        public ICommand NuevaTiendaCommand { get; }
        public ICommand GuardarTiendaCommand { get; }
        public ICommand BorrarTiendaCommand { get; }
        public ICommand NuevaLavadoraCommand { get; }
        public ICommand BorrarLavadoraCommand { get; }
        public ICommand SelectedItemChangedCommand { get; }

        public TiendasCrudViewModel()
        {
            // Inicializamos los atributos
            _listaTiendas = new();
            _listaTiendasVisibles = new();
            _tienda = new Tienda();
            _selected = new Tienda();

            // Inicializamos la lista de estaciones visibles
            ListaTiendasVisibles = new ObservableCollection<Tienda>();

            // Inicializamos los comandos
            CargarCommand = new RelayCommand(PerformCargarTiendas);
            NuevaTiendaCommand = new RelayCommand(PerformNuevaTienda);
            GuardarTiendaCommand = new RelayCommand(PerformGuardarTienda, CanExecuteBotones);
            BorrarTiendaCommand = new RelayCommand(PerformBorrarTienda, CanExecuteBotones);
            NuevaLavadoraCommand = new RelayCommand(PerformNuevaLavadora, CanExecuteBotones);
            BorrarLavadoraCommand = new RelayCommand<Lavadora>(PerformBorrarLavadora);
            SelectedItemChangedCommand = new RelayCommand(PerformSelectedItemChanged);
        }

        #endregion

        #region metodos
        // CanExecuteBotones
        private bool CanExecuteBotones(object? parameter)
        {
            //Debug.WriteLine("CanExecuteBotones");

            // Comprbamos que siempre haya un nombre de Tienda
            bool res = !string.IsNullOrWhiteSpace(Nombre);
            return res;
        }

        // Cargamos las Tiendas
        private async void PerformCargarTiendas(object? parameter = null)
        {
            // Abrimos una conexion con la BBDD
            var service = new ServiceTienda();

            // Cargamos las tiendas desde la BBDD
            _listaTiendas = await service.Listar();

            // Cargamos las tiendas en la ObservableCollection
            ListaTiendasVisibles.Clear();
            foreach (var s in _listaTiendas)
            {
                ListaTiendasVisibles.Add(s);
            }
        }

        // Guardamos la tienda que estamos editando
        private async void PerformGuardarTienda(object? parameter = null)
        {

            // Si la tienda no tiene al menos una lavadora
            if (Lavadoras == null) { return; }

            // Nos conectamos a la BBDD
            var service = new ServiceTienda();

            // Guardamos los cambios dependiendo de si es una nueva tienda o una modificada
            if (Id == 0)
            {
                // Insertamos la nueva tienda
                var nuevaTienda = await service.Insertar(_tienda);
                Id = nuevaTienda.Id;
            }
            else
            {
                var nuevaTienda = await service.Actualizar(_tienda);
            }

            // Guardamos las lavadoras
            var serviceLavadoras = new ServiceLavadora();

            foreach (var lavadora in Lavadoras)
            {
                lavadora.TiendaId = Id;

                if (lavadora.Id == 0)
                {
                    var nuevaLavadora = await serviceLavadoras.Insertar(lavadora);
                }
                else
                {
                    var nuevaLavadora = await serviceLavadoras.Actualizar(lavadora);
                }
            }

            PerformCargarTiendas();
        }

        // Borramos la tienda que estamos editando
        private async void PerformBorrarTienda(object? parameter = null)
        {
            // Nos conectamos a la BBDD
            var service = new ServiceTienda();
            var serviceLavadoras = new ServiceLavadora();

            // Eliminamos las lavadoras asociadas a la tienda seleccionada
            foreach (var lavadora in Lavadoras)
            {
                var lavadoraBorrada = await serviceLavadoras.Borrar(lavadora.Id);
            }

            // Eliminamos la tienda seleccionada
            var tiendaBorrada = await service.Borrar(Id);

            // Recargamos la lista
            PerformCargarTiendas();
            PerformLimpiarTiendas();
        }


        // Añadimos una nueva tienda

        private void PerformNuevaTienda(object? parameter = null)
        {
            // Inicializamos una nueva tienda
            Id = 0;
            Nombre = string.Empty;
            Direccion = string.Empty;
            IsActive = true;
            Lavadoras = new ObservableCollection<Lavadora>();

            // Añadimos una lavadora por defecto a la tienda
            Lavadoras.Add(new Lavadora
            {
                Tipo = 0,
                IsOccupied = false
            });
        }


        // Añadimos una nueva lavadora
        private void PerformNuevaLavadora(object? parameter = null)
        {
            Lavadoras.Add(new Lavadora
            {
                Tipo = 0,
                IsOccupied = false
            });
        }

        // Borramos una lavadora de la lista
        private async void PerformBorrarLavadora(Lavadora lavadora)
        {
            if (lavadora == null) return;
            if (Lavadoras.Count <= 1) { return; }

            Lavadoras.Remove(lavadora);

            if (lavadora.Id != 0)
            {
                var serviceLavadoras = new ServiceLavadora();
                var lavadoraBorrada = await serviceLavadoras.Borrar(lavadora.Id);
            }
        }

        // Manejador de cambio de Item en la lista de tiendas
        private void PerformSelectedItemChanged(object? parameter = null)
        {
            // Comprobamos que haya un objeto seleccionado
            if (parameter != null)
            {
                // Copiamos los datos recibidos desde el SelectedItem de la interfaz a los parametros locales que tenemos en el ViewModel
                Id = Selected.Id;
                Nombre = Selected.Nombre;
                Direccion = Selected.Direccion;
                IsActive = Selected.IsActive;
                Lavadoras = (ObservableCollection<Lavadora>)Selected.Lavadoras;
                Potencia = Selected.Potencia;
            }
        }

        // Limpiador de Interfaz
        private void PerformLimpiarTiendas(object? parameter = null)
        {
            Id = 0;
            Nombre = string.Empty;
            Direccion = string.Empty;
            IsActive = true;
            Lavadoras = new ObservableCollection<Lavadora>();
        }

        #endregion
    }
}
