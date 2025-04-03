﻿using Integrador.Application.DTOs;
using Integrador.Application.Interfaces;
using Integrador.Presentation.Presenters;

namespace Integrador;

public partial class ViewForm : Form
{
    public ViewForm
    (
        IMessenger messenger,
        ICarFactory carFactory,
        IPersonFactory personFactory,
        IViewPresenter viewPresenter,
        IExceptionHandler exceptionHandler
    )
    {
        _messenger = messenger;
        _carFactory = carFactory;
        _personFactory = personFactory;
        _viewPresenter = viewPresenter;
        _exceptionHandler = exceptionHandler;

        try
        {
            InitializeComponent();
            ConfigureBingings();
            LoadData();
        }
        catch (Exception ex)
        {
            _exceptionHandler.Handle(ex, "Error durante la inicialización del formulario.");
        }
    }

    private readonly IMessenger _messenger;
    private readonly ICarFactory _carFactory;
    private readonly IPersonFactory _personFactory;
    private readonly IViewPresenter _viewPresenter;
    private readonly IExceptionHandler _exceptionHandler;

    private readonly BindingSource _persons = [];
    private readonly BindingSource _personCars = [];
    private readonly BindingSource _assignedCars = [];
    private readonly BindingSource _availableCars = [];

    ////////////////////////////////////////////////////////////////////////////

    private void PersonsDataGridView_SelectionChanged(object sender, EventArgs e)
    {
        try
        {
            if (_persons.Current is PersonDTO person)
            {
                _personCars.DataSource = person.Autos;
                _personCars.ResetBindings(false);
                lblCarsPrice.Text = person.GetCarsPrice.ToString("C");
                txtCarsCount.Text = person.GetCarsCount.ToString();
            }
        }
        catch (Exception ex)
        {
            _exceptionHandler.Handle(ex, "Error al seleccionar persona.");
        }
    }

    private void NewPersonButton_Click(object sender, EventArgs e)
    {
        try
        {
            PersonDTO person = _personFactory.CreateDefault();
            _persons.Add(person);
            _persons.MoveLast();
            btnNewPerson.Enabled = false;
        }
        catch (Exception ex)
        {
            _exceptionHandler.Handle(ex, "Error con persona nueva.");
        }
    }

    private async void SavePersonButton_Click(object sender, EventArgs e)
    {
        try
        {
            if (_persons.Current is PersonDTO person)
            {
                await _viewPresenter.SavePerson(person);
                LoadData();
                btnNewCar.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            _exceptionHandler.Handle(ex, "Error al guardar persona.");
        }
    }

    private async void DeletePersonButton_Click(object sender, EventArgs e)
    {
        try
        {
            var confirmacion = _messenger.ShowQuestion("¿Está seguro que desea eliminar la persona seleccionada?", "Eliminar persona");

            if (_persons.Current is PersonDTO persona && confirmacion)
            {
                await _viewPresenter.DeletePerson(persona.Id);
                LoadData();
            }
        }
        catch (Exception ex)
        {
            _exceptionHandler.Handle(ex, "Error al eliminar persona.");
        }
    }

    private async void AssignCarButton_Click(object sender, EventArgs e)
    {
        try
        {
            if (_persons.Current is PersonDTO persona && _availableCars.Current is CarDTO auto)
            {
                await _viewPresenter.AssignCar(persona.Id, auto.Id);
                LoadData();
                _messenger.ShowInformation("Auto asignado correctamente.", "Asignación de auto");
            }
        }
        catch (Exception ex)
        {
            _exceptionHandler.Handle(ex, "Error al asignar auto.");
        }
    }

    private async void RemoveCarButton_Click(object sender, EventArgs e)
    {
        try
        {
            if (_persons.Current is PersonDTO persona && _personCars.Current is CarDTO auto)
            {
                await _viewPresenter.RemoveCar(persona.Id, auto.Id);
                LoadData();
            }
        }
        catch (Exception ex)
        {
            _exceptionHandler.Handle(ex, "Error al desasignar auto.");
        }

    }

    private void NewCarButton_Click(object sender, EventArgs e)
    {
        try
        {
            var newCar = _carFactory.CreateDefault(); // Datos por defecto
            _availableCars.Add(newCar);
            _availableCars.MoveLast();
            btnNewCar.Enabled = false;
        }
        catch (Exception ex)
        {
            _exceptionHandler.Handle(ex, "Error con auto nuevo.");
        }
    }

    private async void SaveCarButton_Click(object sender, EventArgs e)
    {
        try
        {
            if (_availableCars.Current is CarDTO car)
            {
                await _viewPresenter.SaveCar(car);
                LoadData();
                btnNewCar.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            _exceptionHandler.Handle(ex, "Error al guardar auto.");
        }
    }

    private async void DeleteCarButton_Click(object sender, EventArgs e)
    {
        try
        {
            var confirmation = _messenger.ShowQuestion("¿Está seguro que desea eliminar el auto seleccionado?", "Eliminar auto");

            if (_availableCars.Current is CarDTO car && confirmation)
            {
                await _viewPresenter.DeleteCar(car.Id);
                LoadData();
            }
        }
        catch (Exception ex)
        {
            _exceptionHandler.Handle(ex, "Error al eliminar auto.");
        }

    }

    ////////////////////////////////////////////////////////////////////////////

    private void ConfigureBingings()
    {
        ConfigureBindingSources();
        ConfigureDataGridView();
    }

    private void ConfigureBindingSources()
    {
        var bindings = new (Control Control, string Property, BindingSource Source)[]
        {
            // Persons
            (txtPersonId, nameof(PersonDTO.Id),       _persons),
            (txtDNI,      nameof(PersonDTO.DNI),      _persons),
            (txtNombre,   nameof(PersonDTO.Nombre),   _persons),
            (txtApellido, nameof(PersonDTO.Apellido), _persons),
            // Cars
            (txtCarId,   nameof(CarDTO.Id),      _availableCars),
            (txtPatente, nameof(CarDTO.Patente), _availableCars),
            (txtMarca,   nameof(CarDTO.Marca),   _availableCars),
            (txtModelo,  nameof(CarDTO.Modelo),  _availableCars),
            (txtAño,     nameof(CarDTO.Año),     _availableCars),
            (txtPrecio,  nameof(CarDTO.Precio),  _availableCars)
        };

        ConfigureBindingSources(bindings);
    }

    private static void ConfigureBindingSources
    (
        (Control Control, string Property, BindingSource Source)[] bindings
    )
    {
        foreach (var (control, property, source) in bindings)
        {
            control.DataBindings.Add("Text", source, property); // ORIGINAL
        }
    }

    private void ConfigureDataGridView()
    {
        ConfigureDataGridView(dgvPersons, _persons,
        [
            ("Id", "ID"),
            ("DNI", "DNI"),
            ("Nombre", "Nombre"),
            ("Apellido", "Apellido")
        ]);

        ConfigureDataGridView(dgvPersonCars, _personCars,
        [
            ("Id", "ID"),
            ("Patente", "Patente"),
            ("Marca", "Marca"),
            ("Modelo", "Modelo"),
            ("Año", "Año"),
            ("Precio", "Precio")
        ]);

        ConfigureDataGridView(dgvAvailableCars, _availableCars,
        [
            ("Id", "ID"),
            ("Patente", "Patente"),
            ("Marca", "Marca"),
            ("Modelo", "Modelo"),
            ("Año", "Año"),
            ("Precio", "Precio")
        ]);

        ConfigureDataGridView(dgvAssignedCars, _assignedCars,
        [
            ("Marca", "Marca"),
            ("Año", "Año"),
            ("Modelo", "Modelo"),
            ("Patente", "Patente"),
            ("Documento", "Documento"),
            ("Dueño", "Dueño")
        ]);
    }

    private static void ConfigureDataGridView(DataGridView dataGridView,
                                              BindingSource bindingSource,
                                              (string Property, string Header)[] columns)
    {
        dataGridView.AutoGenerateColumns = false;
        dataGridView.DataSource = bindingSource;
        dataGridView.Columns.Clear();

        foreach (var (property, header) in columns)
        {
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = property,
                HeaderText = header,
            });
        }
    }

    private async void LoadData()
    {
        try
        {
            // Asignamos listas vacías antes de la carga real:
            // Dado que la carga es asíncrona, en algún momento entre que el Form
            // se abre y los datos llegan, los DataGridView están en un estado
            // inconsistente. Entonces, cuando los DataGridView intentan acceder
            // a sus columnas antes de que los datos estén completamente
            // cargados, se produce una excepción.
            _persons.DataSource = new List<PersonDTO>();
            _availableCars.DataSource = new List<CarDTO>();
            _assignedCars.DataSource = new List<AssignedCarDTO>();

            _persons.DataSource = await _viewPresenter.ReadPersons();
            _availableCars.DataSource = await _viewPresenter.ReadAvailableCars();
            _assignedCars.DataSource = await _viewPresenter.ReadAssignedCars();
        }
        catch (Exception ex)
        {
            _exceptionHandler.Handle(ex, "Error al cargar datos.");
        }
    }
}
