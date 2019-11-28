using QATool.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilsLib;

namespace QATool.Tests
{
    public class StaminaPotion : Potion
    {
        public override void Run()
        {
            Client client = GetClient(0);

            client = GetClient(0);

            client.EnterCheat("/delall");
            Navigation.EnterGame(client, 0);

            Wait(() => client.inspectorData.Exist("ChangePersonButtonHolder.LobbyMenuButtonFrame(Clone)"), 10);
            Wait(2);

            //идем покупать зелья!
            BuyPotion(client, 2, 20, 0); // зелье стойкости

            client.WaitShowElementGUIAndMouseClick("ButtonBack.Arrow", 1); // назад
            Wait(1);

            client.GoToTestLevel();

            client.EnterConsoleCheat("/statadd 8225 HpAdd", "/statadd -150 HpRegenAdd", "/hp 1");

            //используем зелья и смотрим как они работают
            CheckRemoveBuff(client, new string[] { "/infl 85 100 1" }, 3, 300); // кроветечение

          
            //используем зелья и смотрим что количество уменьшилось на 1
            int oldFirstPotionCount = GetPotionCount(client, 3);
            UsePotionBySlotId(client, 3);
            bool FirstPotionCheck = oldFirstPotionCount - GetPotionCount(client, 3) == 1;


            ExitToLobby(client);

        }

        
    }
}


