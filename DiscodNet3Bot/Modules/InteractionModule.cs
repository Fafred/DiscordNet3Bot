namespace DiscordNet3Bot.Modules
{
    using Discord;
    using Discord.Interactions;
    using Discord.WebSocket;
    using System.Text;

    public class InteractionModule : InteractionModuleBase<SocketInteractionContext>
    {
        [UserCommand("user_command")]
        public async Task HandleUserCommand(IUser user)
        {
            var roles = (user as SocketGuildUser).Roles;

            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"User {user.Mention} has these roles:");

            foreach (var role in roles)
            {
                builder.AppendLine(role.Name);
            }

            await RespondAsync(builder.ToString());
        }

        [MessageCommand("message_command")]
        public async Task HandleMessageCommand(IMessage message)
        {
            await RespondAsync($"{message.Author.Username} has used the message-command");
        }

        [SlashCommand("ping", "Serve it.")]
        public async Task HandlePingCommand()
        {
            await RespondAsync("Pong.");
        }

        [SlashCommand("test", "Testing out interactions.")]
        public async Task HandleTestCommand()
        {
            var button = new ButtonBuilder()
            {
                Label = "This is a button.",
                CustomId = "testbutton01",
                Style = ButtonStyle.Primary,
            };

            var menu = new SelectMenuBuilder()
            {
                CustomId = "testmenu01",
                Placeholder = "Placeholder",
            };

            menu.AddOption("Option #1", "option_1");
            menu.AddOption("Option #2", "option_2");
            menu.AddOption("Option #3", "option_3");

            var component = new ComponentBuilder();
            component.WithButton(button);
            component.WithSelectMenu(menu);

            await RespondAsync("This is the interaction test.", components: component.Build(), ephemeral: true);
        }

        [ComponentInteraction("testbutton01")]
        public async Task HandleTestButton01Input()
        {
            await RespondWithModalAsync<TestModal>("test_modal_01");
        }

        [ComponentInteraction("testmenu01")]
        public async Task HandleTestMenuSelection(string[] inputs)
        {
            await RespondAsync($"You selected {inputs[0]}", ephemeral: true);
        }

        [ModalInteraction("test_modal_01")]
        public async Task HandleTestModal01Input(TestModal modal)
        {
            string input = modal.InputText;
            await RespondAsync(input);
        }
    }

    public class TestModal : IModal
    {
        public string Title => "Test Modal.";

        [InputLabel("Input Label.")]
        [ModalTextInput("test_modal_input", TextInputStyle.Short, placeholder: "This is the placeholder", maxLength: 100)]

        public string InputText { get; set; }
    }
}