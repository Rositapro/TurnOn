using System.Drawing;

namespace TurnOn
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            InicializarSistema();
        }

        private void InicializarSistema()
        {
            ResetAllDevicesVisual(Color.Gray);

            // 2. Inicializa los indicadores y el RadioButton a OFF
            UpdateSystemStatus(false);

            // 3. Inicializa los CheckBoxes de control a desmarcados
            ResetAllDevicesControl(false);

            // 4. Vincular eventos de CheckBox
            VincularEventosCheckBox();
        }
        private void VincularEventosCheckBox()
        {
            // Eventos para Luces
            chkControlLuzPrincipal.CheckedChanged += chkControlLuz_CheckedChanged;
            chkControlLuzRecamara1.CheckedChanged += chkControlLuz_CheckedChanged;
            chkControlLuzRecamara2.CheckedChanged += chkControlLuz_CheckedChanged;
            chkControlLuzSala.CheckedChanged += chkControlLuz_CheckedChanged;
            chkControlLuzCocina.CheckedChanged += chkControlLuz_CheckedChanged;

            // Eventos para Climas
            chkControlClimaPrincipal.CheckedChanged += chkControlClima_CheckedChanged;
            chkControlClimaRecamara1.CheckedChanged += chkControlClima_CheckedChanged;
            chkControlClimaRecamara2.CheckedChanged += chkControlClima_CheckedChanged;
            chkControlClimaSala.CheckedChanged += chkControlClima_CheckedChanged;
            chkControlClimaCocina.CheckedChanged += chkControlClima_CheckedChanged;
        }

        // --- EVENTOS DE CHECKBOX ---
        private void chkControlLuz_CheckedChanged(object sender, EventArgs e)
        {
            ToggleDeviceState((CheckBox)sender, isLight: true);
        }

        private void chkControlClima_CheckedChanged(object sender, EventArgs e)
        {
            ToggleDeviceState((CheckBox)sender, isLight: false);
        }

        // --- LÓGICA DE DISPOSITIVOS ---
        private void ToggleDeviceState(CheckBox controlCheckbox, bool isLight)
        {
            // Bloquea el control si el sistema está apagado
            if (!rbOnOff.Checked)
            {
                controlCheckbox.Checked = false;
                return;
            }

            Button? deviceButton = GetDeviceButton(controlCheckbox, isLight);

            if (deviceButton == null)
            {
                MessageBox.Show("Error de mapeo: CheckBox no corresponde a un botón de estado.", "Error Interno", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Lógica de color CORREGIDA:
            // - CheckBox marcado: VERDE (encendido)
            // - CheckBox desmarcado: ROJO (apagado) - solo si el sistema está activo
            bool newState = controlCheckbox.Checked;
            Color newColor = newState ? Color.LightGreen : Color.Red;

            // Aplica el cambio de color
            deviceButton.BackColor = newColor;
        }

        // --- MÉTODOS AUXILIARES CORREGIDOS ---

        // 1. Mapeo de CheckBox a Button
        private Button? GetDeviceButton(CheckBox controlCheckbox, bool isLight)
        {
            string name = controlCheckbox.Name;

            // Mapeo corregido según los nombres reales
            if (name.Contains("Principal"))
                return isLight ? btnLuzPrincipal : btnClimaPrincipal;
            if (name.Contains("Recamara1"))
                return isLight ? btnLuzRecamara1 : btnClimaRecamara1;
            if (name.Contains("Recamara2"))
                return isLight ? btnLuzRecamara2 : btnClimaRecamara2;
            if (name.Contains("Sala"))
                return isLight ? btnLuzSala : btnClimaSala;
            if (name.Contains("Cocina"))
                return isLight ? btnLuzCocina : btnClimaCocina;

            return null;
        }

        // 2. Actualización de Indicadores CORREGIDA
        private void UpdateSystemStatus(bool isSystemActive)
        {
            Color neutralColor = Color.LightGray;
            Color activeOnColor = Color.Green;
            Color activeOffColor = Color.Red; // CORREGIDO: Apagado en ROJO

            if (isSystemActive)
            {
                // Sistema ON - Indicador Encendido en Verde, Apagado en Rojo
                pnlEncendido.BackColor = activeOnColor;
                pnlApagado.BackColor = activeOffColor; // ROJO cuando el sistema está activo
                pnlFallo.BackColor = neutralColor;

                // Cuando el sistema se activa, todos los botones pasan a ROJO (apagados)
                ResetAllDevicesVisual(Color.Red);
            }
            else
            {
                // Sistema OFF - Todo en colores neutros/gris
                pnlEncendido.BackColor = neutralColor;
                pnlApagado.BackColor = neutralColor;
                pnlFallo.BackColor = neutralColor;

                // Cuando el sistema se desactiva, todos los botones pasan a GRIS
                ResetAllDevicesVisual(Color.Gray);
            }
        }

        // 3. Reset Visual de Botones
        private void ResetAllDevicesVisual(Color color)
        {
            Button?[] deviceButtons = {
                btnLuzPrincipal, btnClimaPrincipal,
                btnLuzRecamara1, btnClimaRecamara1,
                btnLuzRecamara2, btnClimaRecamara2,
                btnLuzSala, btnClimaSala,
                btnLuzCocina, btnClimaCocina
            };

            foreach (Button? btn in deviceButtons)
            {
                if (btn != null)
                {
                    btn.BackColor = color;
                }
            }
        }

        // 4. Reset de CheckBoxes de Control
        private void ResetAllDevicesControl(bool checkState)
        {
            CheckBox?[] controlCheckboxes = {
                chkControlLuzPrincipal, chkControlClimaPrincipal,
                chkControlLuzRecamara1, chkControlClimaRecamara1,
                chkControlLuzRecamara2, chkControlClimaRecamara2,
                chkControlLuzSala, chkControlClimaSala,
                chkControlLuzCocina, chkControlClimaCocina
            };

            foreach (CheckBox? chk in controlCheckboxes)
            {
                if (chk != null)
                {
                    chk.Checked = checkState;
                }
            }
        }
        private void rbOnOff_CheckedChanged_1(object sender, EventArgs e)
        {
            bool isSystemOn = rbOnOff.Checked;
            HandleSystemToggle(isSystemOn);
        }
        private void HandleSystemToggle(bool isSystemOn)
        {
            if (isSystemOn)
            {
                // Sistema ON
                UpdateSystemStatus(true);
            }
            else // Estado OFF
            {
                // Sistema OFF
                UpdateSystemStatus(false);
                ResetAllDevicesControl(false); // Desmarca todos los CheckBoxes
                ResetAllDevicesVisual(Color.Gray); // Pone todos los botones de estado en Gris
            }
        }
    }
}

