﻿using CarAssignment.Application.Authentication;
using CarAssignment.Application.Authentication.State;
using CarAssignment.Domain.Entities;
using CarAssignment.Presentation.Composition;

using System.ComponentModel;

namespace Integrador.Presentation.Views;

public partial class LoginForm : Form
{
    private readonly IAuthenticationService _authenticationService = AppServiceProvider.GetService<IAuthenticationService>();

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public User CurrentUser { get; private set; } = default!;

    public LoginForm()
    {
        InitializeComponent();
    }

    private void ButtonLogin_Click(object sender, EventArgs e)
    {
        var username = txtUsername.Text.Trim();
        var password = txtPassword.Text.Trim();
        var user = _authenticationService.Authenticate(username, password);

        if (user is not null)
        {
            CurrentUser = user;
            Session.Start(user);
            DialogResult = DialogResult.OK;
            Close();
        }
        else
        {
            MessageBox.Show("Usuario o contraseña incorrectos.");
        }
    }
}
