using System.Drawing;

namespace TurnOn
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
            ResetAllDevicesVisual(Color.White);
            UpdateSystemStatus(false);
            rbOnOff.Checked = false;
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }
        private void rbOnOff_CheckedChanged(object sender, EventArgs e)
        {
            bool isSystemOn = rbOnOff.Checked;

            if (isSystemOn)
            {
                UpdateSystemStatus(true);
                MessageBox.Show("Controladores activados. Sistema operativo (ON).", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else // Estado OFF
            {
                UpdateSystemStatus(false);
                // ✅ Estas llamadas ya no generarán error CS0103 una vez se definan los métodos
                ResetAllDevicesControl(false);
                ResetAllDevicesVisual(Color.White);
                MessageBox.Show("Sistema de Control en estado OFF. Dispositivos desactivados.", "Estado OFF", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // --- Manejo de Dispositivos ---

        private void ToggleDeviceState(CheckBox controlCheckbox, bool isLight)
        {
            if (!rbOnOff.Checked)
            {
                MessageBox.Show("El sistema de Controladores no está activo (OFF). Active el sistema con 'On / Off' primero.", "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                controlCheckbox.Checked = false;
                return;
            }

            // ✅ El error CS8602 (posiblemente NULL) se maneja con el 'if (deviceButton == null)'
            Button deviceButton = GetDeviceButton(controlCheckbox, isLight);
            if (deviceButton == null)
            {
                // Si deviceButton es null, significa que un CheckBox no está mapeado correctamente.
                MessageBox.Show("Error de mapeo: el CheckBox no corresponde a un botón de estado.", "Error Interno", MessageBoxButtons.OK, MessageBoxIcon.Error);
                controlCheckbox.Checked = false;
                return;
            }

            bool newState = controlCheckbox.Checked;
            Color newColor = newState ? Color.LightGreen : Color.White;
            string newText = newState ? "ON" : "OFF";
            string deviceType = isLight ? "Luz" : "Clima";

            // ✅ Asegúrate de que el control padre exista antes de acceder a su Text
            string roomName = deviceButton.Parent != null ? deviceButton.Parent.Text : "Habitación Desconocida";

            deviceButton.BackColor = newColor;
            deviceButton.Text = newText;

            MessageBox.Show($"{deviceType} de {roomName} ha sido: {(newState ? "ENCENDIDA" : "APAGADA")}.",
                            "Control de Dispositivo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void chkControlLuz_CheckedChanged(object sender, EventArgs e)
        {
            ToggleDeviceState((CheckBox)sender, isLight: true);
        }

        private void chkControlClima_CheckedChanged(object sender, EventArgs e)
        {
            ToggleDeviceState((CheckBox)sender, isLight: false);
        }

        // --- MÉTODOS AUXILIARES (¡DEBES AÑADIR ESTOS PARA SOLUCIONAR LOS ERRORES CS0103!) ---

        // 1. Método para mapear el CheckBox de control al Button de estado
        private Button GetDeviceButton(CheckBox controlCheckbox, bool isLight)
        {
            string name = controlCheckbox.Name;
            // IMPORTANTE: Asegúrate que los nombres de tus controles coincidan con esta lógica
            // Por ejemplo, el CheckBox de control de luz de la Recamara Principal debe llamarse
            // 'chkControlLuzPrincipal' y el botón de estado debe llamarse 'btnLuzPrincipal'.
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
            return null; // Retorna null si no encuentra un mapeo
        }

        // 2. Método para actualizar los colores de los Paneles Indicadores
        private void UpdateSystemStatus(bool isOn)
        {
            if (isOn)
            {
                pnlEncendido.BackColor = Color.Green;
                pnlApagado.BackColor = Color.Gray;
                pnlFallo.BackColor = Color.Gray;
            }
            else
            {
                pnlEncendido.BackColor = Color.Gray;
                pnlApagado.BackColor = Color.DarkGray;
                pnlFallo.BackColor = Color.Gray;
            }
            // Esto es opcional, ya que este método es llamado por rbOnOff_CheckedChanged.
            // rbOnOff.Checked = isOn; 
        }

        // 3. Método para resetear visualmente los botones de estado (CS0103)
        private void ResetAllDevicesVisual(Color color)
        {
            // Debes asegurarte que TODOS estos nombres de botones existan en tu Formulario.
            Button[] deviceButtons = {
                btnLuzPrincipal, btnClimaPrincipal,
                btnLuzRecamara1, btnClimaRecamara1,
                btnLuzRecamara2, btnClimaRecamara2,
                btnLuzSala, btnClimaSala,
                btnLuzCocina, btnClimaCocina
            };

            foreach (Button btn in deviceButtons)
            {
                // Maneja la posible referencia NULL por si algún botón no existe en el diseñador
                if (btn != null)
                {
                    btn.BackColor = color;
                    btn.Text = "OFF";
                }
            }
        }

        // 4. Método para resetear los CheckBoxes de control (CS0103)
        private void ResetAllDevicesControl(bool checkState)
        {
            // Debes asegurarte que TODOS estos nombres de CheckBoxes existan en tu Formulario.
            CheckBox[] controlCheckboxes = {
                chkControlLuzPrincipal, chkControlClimaPrincipal,
                chkControlLuzRecamara1, chkControlClimaRecamara1,
                chkControlLuzRecamara2, chkControlClimaRecamara2,
                chkControlLuzSala, chkControlClimaSala,
                chkControlLuzCocina, chkControlClimaCocina
            };

            foreach (CheckBox chk in controlCheckboxes)
            {
                // Maneja la posible referencia NULL por si algún CheckBox no existe en el diseñador
                if (chk != null)
                {
                    chk.Checked = checkState;
                }
            }
        }
    }
}

