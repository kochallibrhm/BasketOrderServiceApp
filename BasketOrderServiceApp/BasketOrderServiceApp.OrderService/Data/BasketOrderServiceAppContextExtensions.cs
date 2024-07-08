namespace BasketOrderServiceApp.OrderService.Data.Entity;

public static class BasketOrderServiceAppContextExtensions {
    public static async Task Initialize(this BasketOrderServiceAppContext context, IHashService hashService) {
        await context.Database.EnsureCreatedAsync();

        var currentUsers = await context.Users.ToListAsync();

        bool anyNewUser = false;

        if (!currentUsers.Any(u => u.UserName == "User1")) {
            context.Users.Add(new User {
                UserName = "User1",
                Password = await hashService.HashText("Password1")
            });

            anyNewUser = true;
        }

        if (!currentUsers.Any(u => u.UserName == "User2")) {
            context.Users.Add(new User {
                UserName = "User2",
                Password = await hashService.HashText("Password2")
            });

            anyNewUser = true;
        }

        if (!currentUsers.Any(u => u.UserName == "User3")) {
            context.Users.Add(new User {
                UserName = "User3",
                Password = await hashService.HashText("Password3")
            });

            anyNewUser = true;
        }

        if (!currentUsers.Any(u => u.UserName == "User4")) {
            context.Users.Add(new User {
                UserName = "User4",
                Password = await hashService.HashText("Password4")
            });

            anyNewUser = true;
        }

        if (anyNewUser) {
            await context.SaveChangesAsync();
        }
    }
}
