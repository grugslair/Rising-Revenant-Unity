using UnityEngine;
using UnityEditor;
using Dojo; // Ensure this is included to access your namespace

[CustomEditor(typeof(WorldManagerData))]
public class WorldManagerDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // Draw the default inspector

        WorldManagerData data = (WorldManagerData)target;

        if (GUI.changed) // Check if any inspector value has changed
        {
            switch (data.environmentType) // Check the environment type
            {
                case EnvironmentType.LOCAL:

                    data.toriiUrl = "http://localhost:8080";
                    data.rpcUrl = "http://localhost:5050";

                    data.worldAddress = "0x7a4adc3dc01142811d2db99848998828b30e8a3c3d2a3875751f427ff11ad35";

                    data.gameActionsAddress = "0x54017a9219f6483af430c0aa45dd5d6d1fa129d768b01d427636a5121b5f285";
                    data.eventActionsAddress = "0x1f69f004d6bfd4e09b328cff8e84ad9a73d6b2b034741b69ec200d30fc7df97";
                    data.outpostActionsAddress = "0x71977a0f344a88cd338279dc01795428ce9872062f353eea3cc2d5ade58cf40";
                    data.reinforcementActionsAddress = "0x64d6e5d52b28941226a0a4148a6a26f8fd4271a5150b2cfe49ab6f64788bff2";
                    data.paymentActionsAddress = "0x1cbf7df5c43cefe7d497b5e8985e566c0ef910f600a1ea0ebb2efcd34577d99";
                    data.tradeOutpostActionsAddress = "0x76a4dc474308a5ea36e3b089940269d90a6b4c6dcb98acb6f6018a177e1a827";
                    data.tradeReinforcementActionsAddress = "0x696cde8aa85d7e3191eab7396640f11a002c68dc11fe6cb6dc94471bd35ff90";

                    data.masterAddress = "0xb3ff441a68610b30fd5e2abbf3a1548eb6ba6f3559f2862bf2dc757e5828ca";
                    data.masterPrivateKey = "0x2bbf4f9fd0bbb2e60b0316c1fe0b76cf7a4d0198bd493ced9b8df2a3a24d68a";
                    break;

                case EnvironmentType.TORII:

                    data.toriiUrl = "https://api.cartridge.gg/x/rr/torii";
                    data.rpcUrl = "https://api.cartridge.gg/x/rr/katana";

                    data.worldAddress = "0x739075d3eab7b1463ef8e99ad59afc470dbee7a4d5682fecde6c84c0798e1e7";
                    data.gameActionsAddress = "0x2bbd7c9d7822223e43460fe0c0c8089cab0f4e5c6b960ae7ec38a9ef8e560b2";
                    data.eventActionsAddress = "0x223e3880bb801613e3ad10efc0e3b54f83e9d9a64100c62148dad450178e272";
                    data.outpostActionsAddress = "0x1ce589283a7353cad2c51f9c7ef2407962ab356cfb32f909ae0dfba315f342b";
                    data.reinforcementActionsAddress = "0x7c68de0e3b99ffc19673104c582653b786d16eb18bd721716dcf2bfe5785d2f";
                    data.paymentActionsAddress = "0x7cfca1aa08b1ef734b532c8b16d4e267c41451b9a4b7ba9c5e557400324a102";
                    data.tradeOutpostActionsAddress = "0x4b595653ed5a8e53fafb6c45904eea086ce57eed8082970572a9b5163a4f9a5";
                    data.tradeReinforcementActionsAddress = "0x677359fd0f14a9d4af8cac6e29354a1a0380bde387241deabd27560bf475d95";

                    data.masterAddress = "0x3ebe00c0bce66b6d4bb20726812bff83fbb527226babcaf3d4dac46915cedb";
                    data.masterPrivateKey = "0x1d2ce4b504f4dcf9061d4db9e10e9d5d14f37b4ec595a648d6cd6e005ef937e";
                    break;

                case EnvironmentType.TESTNET:

                    data.toriiUrl = "https://api.cartridge.gg/x/sepoliarr/torii";
                    data.rpcUrl = "https://starknet-sepolia.public.blastapi.io/rpc/v0_6";

                    data.worldAddress = "0x7a4adc3dc01142811d2db99848998828b30e8a3c3d2a3875751f427ff11ad35";

                    data.gameActionsAddress = "0x54017a9219f6483af430c0aa45dd5d6d1fa129d768b01d427636a5121b5f285";
                    data.eventActionsAddress = "0x1f69f004d6bfd4e09b328cff8e84ad9a73d6b2b034741b69ec200d30fc7df97";
                    data.outpostActionsAddress = "0x71977a0f344a88cd338279dc01795428ce9872062f353eea3cc2d5ade58cf40";
                    data.reinforcementActionsAddress = "0x64d6e5d52b28941226a0a4148a6a26f8fd4271a5150b2cfe49ab6f64788bff2";
                    data.paymentActionsAddress = "0x1cbf7df5c43cefe7d497b5e8985e566c0ef910f600a1ea0ebb2efcd34577d99";
                    data.tradeOutpostActionsAddress = "0x76a4dc474308a5ea36e3b089940269d90a6b4c6dcb98acb6f6018a177e1a827";
                    data.tradeReinforcementActionsAddress = "0x696cde8aa85d7e3191eab7396640f11a002c68dc11fe6cb6dc94471bd35ff90";

                    data.masterAddress = "";
                    data.masterPrivateKey = "";

                    break;
            }

            EditorUtility.SetDirty(data); 
        }
    }
}
