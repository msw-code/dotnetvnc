//  Copyright (C) 2005 Rocky Lo. All Rights Reserved.
//
//  This file is part of the VNC system.
//
//  The VNC system is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307,
//  USA.
//
// If the source code for the VNC system is not available from the place 
// whence you received this file, check http://www.uk.research.att.com/vnc or contact
// the authors on vnc@uk.research.att.com for information on obtaining it.

using System;

namespace Vnc.Viewer
{
  internal class SessDlgFactory
  {
    internal static SessDlg Create()
    {
      if(App.DevCap.Lvl >= DevCapLvl.Desktop)
        return new SessDlgDt();
      else if(App.DevCap.Lvl >= DevCapLvl.PocketPc)
        return new SessDlgPpc();
      else if(App.DevCap.Lvl >= DevCapLvl.Smartphone)
        return new SessDlgSp();
      else
        throw new NotSupportedException(App.GetStr("Device capability is unknown."));
    }

    internal static SessDlg Create(ViewOpts viewOpts)
    {
      if(App.DevCap.Lvl >= DevCapLvl.Desktop)
        return new SessDlgDt(viewOpts);
      else if(App.DevCap.Lvl >= DevCapLvl.PocketPc)
        return new SessDlgPpc(viewOpts);
      else if(App.DevCap.Lvl >= DevCapLvl.Smartphone)
        return new SessDlgSp(viewOpts);
      else
        throw new NotSupportedException(App.GetStr("Device capability is unknown."));
    }
  }
}
