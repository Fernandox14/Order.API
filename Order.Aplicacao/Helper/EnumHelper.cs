using Order.Aplicacao.DTO;
using Order.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Order.Aplicacao.Helper;

public static class EnumHelper
{
    public static string ToDescription(this ProductStatus status)
    {
        var field = status.GetType().GetField(status.ToString());

        var attribute = field?.GetCustomAttribute<EnumTextAttribute>();

        return attribute?.Text ?? status.ToString();
    }

    public static string ToDescription(this ReservationStatus status)
    {
        var field = status.GetType().GetField(status.ToString());

        var attribute = field?.GetCustomAttribute<EnumTextAttribute>();

        return attribute?.Text ?? status.ToString();
    }
}
