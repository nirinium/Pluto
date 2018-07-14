using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PUBG_Mouse_Helper
{
    public class Poller
    {
        private IOnHotkeyPressed onHotkeyPressed;

        public Poller(IOnHotkeyPressed onHotkeyPressed)
        {
            this.onHotkeyPressed = onHotkeyPressed;
        }

        public bool PerformRecoilCompensation { get; set; } = true;
        public bool PerformRapidfire { get; set; } = true;
        public bool PerformFastLoot { get; set; } = true;

        public void Poll(int dx, int dy, uint sleep)
        {
            PollMButton(dx, dy, sleep);
            if (this.PerformRecoilCompensation)
            {
                PollPresetChangeHotkey();
                PollTrackbarValuesChangeHotkey();
            }
            PollToggleRecoilCompensationHotkey();
            //PollFastLoot();
            //PollRapidfire();
        }

        //TEST
        public void Poll_ExtraFunctions()
        {
            PollFastLoot();
            PollRapidfire();
        }

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        private void PollFastLoot()
        {
            short gaks = GetAsyncKeyState(Keys.XButton1);
            
            if ((gaks & 0b10000000_00000000) > 0)//if FastLoot was pressed and held
            {
                HelperFunctions.WaitUntilTimeoutWhileTrue(() => (GetAsyncKeyState(Keys.XButton1) & 0b10000000_00000000) > 0, 100); //wait until F7 arrow key is released without timeout of 100ms
                this.onHotkeyPressed.OnFastLootPressed();
            }
        }

        private void PollMButton(int dx, int dy, uint sleep)
        {
            short gaks = GetAsyncKeyState(System.Windows.Forms.Keys.LButton);
            if ((gaks & 0b10000000_00000000) > 0) //if MSB is set (non-zero) i.e. left button is being held down
            {
                //Could add rapidfire here -- have to use KEYBD_EVENT
                //MouseHelperClass.LeftClickDown();
                //MouseHelperClass.LeftClickUp();                
                if (this.PerformRecoilCompensation)
                {
                    MouseHelperClass.MouseMove(dx, dy, sleep);
                }


                /* else if (this.PerformRecoilCompensation != true)
                {
                    MouseHelperClass.Rapidfire();
                }
                //add Single Shot Rapidfire Here (else)
                */
            }
        }

        private void PollPresetChangeHotkey()
        {
            short gaks = GetAsyncKeyState(Keys.NumPad9);
            if ((gaks & 0b10000000_00000000) > 0) //if Enter was pressed and held
            {
                HelperFunctions.WaitUntilTimeoutWhileTrue(() => (GetAsyncKeyState(Keys.NumPad9) & 0b10000000_00000000) > 0, 100); //wait until Enter is released without timeout of 100ms
                this.onHotkeyPressed.OnPresetSwitchHotkeyPressed();
            }
        }

        private void PollRapidfire()
        {
            short gaks = GetAsyncKeyState(Keys.XButton2);
            if ((gaks & 0b10000000_00000000) > 0)//if FastLoot was pressed and held
            {
                HelperFunctions.WaitUntilTimeoutWhileTrue(() => (GetAsyncKeyState(Keys.XButton2) & 0b10000000_00000000) > 0, 100); //wait until F7 arrow key is released without timeout of 100ms
                this.onHotkeyPressed.OnRapidfirePressed();
            }
        }

        private void PollToggleRecoilCompensationHotkey()
        {
            short gaks = GetAsyncKeyState(Keys.F7);
            if ((gaks & 0b10000000_00000000) > 0)//if F7 arrow key was pressed and held
            {
                HelperFunctions.WaitUntilTimeoutWhileTrue(() => (GetAsyncKeyState(Keys.F7) & 0b10000000_00000000) > 0, 100); //wait until F7 arrow key is released without timeout of 100ms
                this.onHotkeyPressed.OnToggleRecoilCompensationHotkeyPressed();
            }
        }

        private void PollTrackbarValuesChangeHotkey()
        {
            short gaks = GetAsyncKeyState(Keys.Right);
            if ((gaks & 0b10000000_00000000) > 0)//if Right arrow key was pressed and held
            {
                HelperFunctions.WaitUntilTimeoutWhileTrue(() => (GetAsyncKeyState(Keys.Right) & 0b10000000_00000000) > 0, 100); //wait until Right arrow key is released without timeout of 100ms
                this.onHotkeyPressed.OnRightArrowPressed();
            }

            gaks = GetAsyncKeyState(Keys.Left);
            if ((gaks & 0b10000000_00000000) > 0)//if Left arrow key was pressed and held
            {
                HelperFunctions.WaitUntilTimeoutWhileTrue(() => (GetAsyncKeyState(Keys.Left) & 0b10000000_00000000) > 0, 100); //wait until Left arrow key is released without timeout of 100ms
                this.onHotkeyPressed.OnLeftArrowPressed();
            }

            gaks = GetAsyncKeyState(Keys.Up);
            if ((gaks & 0b10000000_00000000) > 0)//if Up arrow key was pressed and held
            {
                HelperFunctions.WaitUntilTimeoutWhileTrue(() => (GetAsyncKeyState(Keys.Up) & 0b10000000_00000000) > 0, 100); //wait until Up arrow key is released without timeout of 100ms
                this.onHotkeyPressed.OnUpArrowPressed();
            }

            gaks = GetAsyncKeyState(Keys.Down);
            if ((gaks & 0b10000000_00000000) > 0)//if Down arrow key was pressed and held
            {
                HelperFunctions.WaitUntilTimeoutWhileTrue(() => (GetAsyncKeyState(Keys.Down) & 0b10000000_00000000) > 0, 100); //wait until Down arrow key is released without timeout of 100ms
                this.onHotkeyPressed.OnDownArrowPressed();
            }
        }
    }
}