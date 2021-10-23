﻿using Dalamud.Game.Command;
using Dalamud.Plugin;
using Dalamud.IoC;
using System;
using System.IO;
using System.Reflection;
{%- if cookiecutter.di_scheme == "constructor" %}
using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Buddy;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Fates;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.ClientState.Party;
using Dalamud.Game.Gui;
using Dalamud.Game.Gui.FlyText;
using Dalamud.Game.Gui.PartyFinder;
using Dalamud.Game.Gui.Toast;
using Dalamud.Game.Libc;
using Dalamud.Game.Network;
using Dalamud.Game.Text.SeStringHandling;
{%- endif %}

namespace {{cookiecutter.project_name}}
{
    public class {{cookiecutter.project_name}} : IDalamudPlugin
    {
        public string Name => "{{cookiecutter.public_name}}";

        private const string CommandName = "/{{cookiecutter.main_command}}";

        {% if cookiecutter.di_scheme == "constructor" -%}
        public static DalamudPluginInterface PluginInterface { get; private set; }
        public static CommandManager CommandManager { get; private set; }
        public static SigScanner SigScanner { get; private set; }
        public static DataManager DataManager { get; private set; }
        public static ClientState ClientState { get; private set; }
        public static ChatGui ChatGui { get; private set; }
        public static ChatHandlers ChatHandlers { get; private set; }
        public static Framework Framework { get; private set; }
        public static GameNetwork GameNetwork { get; private set; }
        public static Condition Condition { get; private set; }
        public static KeyState KeyState { get; private set; }
        public static GameGui GameGui { get; private set; }
        public static FlyTextGui FlyTextGui { get; private set; }
        public static ToastGui ToastGui { get; private set; }
        public static JobGauges JobGauges { get; private set; }
        public static PartyFinderGui PartyFinderGui { get; private set; }
        public static BuddyList BuddyList { get; private set; }
        public static PartyList PartyList { get; private set; }
        public static TargetManager TargetManager { get; private set; }
        public static ObjectTable ObjectTable { get; private set; }
        public static FateTable FateTable { get; private set; }
        public static LibcFunction LibcFunction { get; private set; }

        {% elif cookiecutter.di_scheme == "none" -%}
        public static DalamudPluginInterface PluginInterface { get; private set; }
        public static CommandManager CommandManager { get; private set; }

        {% endif -%}
        
        private Configuration Configuration { get; init; }
        private {{cookiecutter.project_name}}UI UI { get; init; }

        public {{cookiecutter.project_name}}({% if cookiecutter.di_scheme == "container" -%}
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface
            {%- elif cookiecutter.di_scheme == "constructor" %}
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager,
            [RequiredVersion("1.0")] SigScanner sigScanner,
            [RequiredVersion("1.0")] DataManager dataManager,
            [RequiredVersion("1.0")] ClientState clientState,
            [RequiredVersion("1.0")] ChatGui chatGui,
            [RequiredVersion("1.0")] ChatHandlers chatHandlers,
            [RequiredVersion("1.0")] Framework framework,
            [RequiredVersion("1.0")] GameNetwork gameNetwork,
            [RequiredVersion("1.0")] Condition condition,
            [RequiredVersion("1.0")] KeyState keyState,
            [RequiredVersion("1.0")] GameGui gameGui,
            [RequiredVersion("1.0")] FlyTextGui flyTextGui,
            [RequiredVersion("1.0")] ToastGui toastGui,
            [RequiredVersion("1.0")] JobGauges jobGauges,
            [RequiredVersion("1.0")] PartyFinderGui partyFinderGui,
            [RequiredVersion("1.0")] BuddyList buddyList,
            [RequiredVersion("1.0")] PartyList partyList,
            [RequiredVersion("1.0")] TargetManager targetManager,
            [RequiredVersion("1.0")] ObjectTable objectTable,
            [RequiredVersion("1.0")] FateTable fateTable,
            [RequiredVersion("1.0")] LibcFunction libcFunction
            {%- else %}
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager
            {%- endif %})
        {
            {%- if cookiecutter.di_scheme == "constructor" %}
            PluginInterface = pluginInterface;
            CommandManager = commandManager;
            SigScanner = sigScanner;
            DataManager = dataManager;
            ClientState = clientState;
            ChatGui = chatGui;
            ChatHandlers = chatHandlers;
            Framework = framework;
            GameNetwork = gameNetwork;
            Condition = condition;
            KeyState = keyState;
            GameGui = gameGui;
            FlyTextGui = flyTextGui;
            ToastGui = toastGui;
            JobGauges = jobGauges;
            PartyFinderGui = partyFinderGui;
            BuddyList = buddyList;
            PartyList = partyList;
            TargetManager = targetManager;
            ObjectTable = objectTable;
            FateTable = fateTable;
            LibcFunction = libcFunction;
            {%- elif cookiecutter.di_scheme == "container" %}
            DalamudContainer.Initialize(pluginInterface);
            {%- else %}
            PluginInterface = pluginInterface;
            CommandManager = commandManager;
            {%- endif %}

            {% if cookiecutter.di_scheme == "container" -%}
                {% set interface_reference = "DalamudContainer.PluginInterface" %}
            {%- else -%}
                {% set interface_reference = "PluginInterface" %}
            {%- endif -%}
            {%- if cookiecutter.di_scheme == "container" -%}
                {% set command_reference = "DalamudContainer.CommandManager" %}
            {%- else -%}
                {% set command_reference = "CommandManager" %}
            {%- endif -%}
            
            this.Configuration = {{ interface_reference }}.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize({{ interface_reference }});
            {% if cookiecutter.include_goat == "yes" %}
            {% if cookiecutter.include_comments == "yes" -%}
            // you might normally want to embed resources and load them from the manifest stream
            {% endif -%}
            var imagePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"goat.png");
            var goatImage = {{ interface_reference }}.UiBuilder.LoadImage(imagePath);
            this.UI = new {{cookiecutter.project_name}}UI(this.Configuration, goatImage);
            {% else %}
            this.UI = new {{cookiecutter.project_name}}UI(this.Configuration);
            {% endif %}
            {{ command_reference }}.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "{{cookiecutter.main_command_help}}"
            });

            {{ interface_reference }}.UiBuilder.Draw += DrawUI;
            {{ interface_reference }}.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void Dispose()
        {
            this.UI.Dispose();

            {{ command_reference }}.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            {% if cookiecutter.include_comments == "yes" -%}
            // in response to the slash command, just display our main ui
            {% endif -%}
            this.UI.Visible = true;
        }

        private void DrawUI()
        {
            this.UI.Draw();
        }

        private void DrawConfigUI()
        {
            this.UI.SettingsVisible = true;
        }
    }
}
