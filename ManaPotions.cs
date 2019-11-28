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
    public class ManaPotions : Potion
    {
        public override void Run()
        {
            Client client = GetClient(0);

            client = GetClient(0);

            client.EnterCheat("/delall");
            Navigation.EnterGame(client, 0);

            Wait(() => client.inspectorData.Exist("ChangePersonButtonHolder.LobbyMenuButtonFrame(Clone)"), 10);

            client.EnterCheat("/level 10", "/goldadd 1000000", "/emerald 100000");

            //идем покупать зелья!
            BuyPotion(client, 2, 5, 0);
            BuyPotion(client, 2, 6, 1);
            BuyPotion(client, 2, 7, 2);
            BuyPotion(client, 2, 8, 3);
            client.WaitShowElementGUIAndMouseClick("ButtonBack.Arrow", 1);

            client.GoToTestLevel();
            client.EnterCheat("/statadd 100000 SpAdd", "/statadd -1 SafeZone", "/sp 1"); 

            //используем зелья и смотрим сколько хп они восстанавливают
            UseManaAndEquale(client, 3, 0);
            client.EnterCheat("/hp 1");
            UseManaAndEquale(client, 2, 1);
            client.EnterCheat("/hp 1");
            UseManaAndEquale(client, 1, 2);
            client.EnterCheat("/hp 1");
            UseManaAndEquale(client, 0, 3);


            //используем зелья и смотрим что количество уменьшилось на 1
            int oldFourthPotionCount = GetPotionCount(client, 3);
            int oldThirdPotionCount = GetPotionCount(client, 2);
            int oldSecondPotionCount = GetPotionCount(client, 1);
            int oldFirstPotionCount = GetPotionCount(client, 0);

            UsePotionBySlotId(client, 3);
            UsePotionBySlotId(client, 2);
            UsePotionBySlotId(client, 1);
            UsePotionBySlotId(client, 0);

            bool FourthPotionCheck = oldFourthPotionCount - GetPotionCount(client, 3) == 1;
            bool ThirdPotionCheck = oldThirdPotionCount - GetPotionCount(client, 2) == 1;
            bool SecondPotionCheck = oldSecondPotionCount - GetPotionCount(client, 1) == 1;
            bool FirstPotionCheck = oldFirstPotionCount - GetPotionCount(client, 0) == 1;



            ExitToLobby(client);

            Wait(() => client.inspectorData.Exist("ChangePersonButtonHolder.LobbyMenuButtonFrame(Clone)"), 10);
            client.EnterCheat("/getitem 116 3"); // чемпионское зелье маны
            Wait(1);
            client.WaitShowElementGUIAndMouseClick("ChangePersonButtonHolder.LobbyMenuButtonFrame(Clone)", 3); // арсенал   
            Wait(1);
            client.WaitShowElementGUIAndMouseClick("LeftMenuButtonFramePW3(Clone)", 3, 3); // зелья
            Wait(1);
            client.WaitShowElementGUIAndMouseClick("ItemFramePW3(Clone)", 9, 3); // чемпионское зелье маны
            Wait(1);
            if (client.inspectorData.Exist("NotificationForm"))
            {
                client.WaitShowElementGUIAndMouseClick("ButtonBack.Arrow", 1); // назад
            }
            client.WaitShowElementGUIAndMouseClick("PotionSlotsHolder.ItemFramePW3(Clone)", 3, 3); // 4й слот в поясе
            Wait(1);
            client.WaitShowElementGUIAndMouseClick("ButtonBack.Arrow", 1); // назад
            Wait(1);
            client.GoToTestLevel();
            client.EnterCheat("/statadd 100000 SpAdd", "/statadd -1 SafeZone", "/sp 1");

            UseManaAndEquale(client, 0, 4);
            ExitToLobby(client);
        }


       
    }
}

