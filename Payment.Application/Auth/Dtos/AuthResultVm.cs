﻿namespace Payment.Application.Auth.Dtos;

public class AuthResultVm
{
    public string JwtToken { get; set; }

    public string UserId { get; set; }

    public string Nickname { get; set; }
}
