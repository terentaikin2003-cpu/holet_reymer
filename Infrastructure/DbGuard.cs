using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelReymer.Infrastructure;

/// <summary>
/// Единообразная обработка ошибок БД при удалении (FK и др.).
/// </summary>
public static class DbGuard
{
    public static async Task<IActionResult> RunDeleteAsync(Controller controller, Func<Task> deleteWork)
    {
        try
        {
            await deleteWork();
            return controller.RedirectToAction("Index");
        }
        catch (DbUpdateException)
        {
            controller.TempData["Error"] =
                "Невозможно удалить запись: существуют связанные данные в других таблицах.";
            return controller.RedirectToAction("Index");
        }
    }
}
