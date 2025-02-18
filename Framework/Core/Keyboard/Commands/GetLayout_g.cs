/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * GetLayout_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Keyboard.Commands
{
    //Original name = GetLayout
    [DataContract]
    [Command(Name = "Keyboard.GetLayout")]
    public sealed class GetLayoutCommand : Command<GetLayoutCommand.PayloadData>
    {
        public GetLayoutCommand(int RequestId, GetLayoutCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, EntryModeEnum? EntryMode = null)
                : base(Timeout)
            {
                this.EntryMode = EntryMode;
            }

            public enum EntryModeEnum
            {
                Data,
                Pin,
                Secure
            }

            /// <summary>
            /// Specifies entry mode to be returned.
            /// If this property is omitted, all supported layouts will be returned.
            /// The following values are possible:
            /// 
            /// * ```data``` - Get the layout for the [Keyboard.DataEntry](#keyboard.dataentry) command.
            /// * ```pin``` - Get the layout for the [Keyboard.PinEntry](#keyboard.pinentry) command.
            /// * ```secure``` - Get the layout for the [Keyboard.SecureKeyEntry](#keyboard.securekeyentry) command.
            /// </summary>
            [DataMember(Name = "entryMode")]
            public EntryModeEnum? EntryMode { get; init; }

        }
    }
}
