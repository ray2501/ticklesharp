/*
 * tclwrapper.c - TickleTkSharp .NET class support functions.
 *
 * Copyright (C) 2003  Scott Beasley.
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

#include <stdlib.h>
#include <string.h>
#include <tcl.h>
#include <tk.h>
#ifdef __UNIX__
#include <unistd.h>
#endif
#include <locale.h>
#ifdef __WIN32__
#include <windows.h>
#endif

#if __WORDSIZE == 64           
# define BUILD_64   1
#endif

int Tcl_WrapperCmdProc (ClientData clientData, Tcl_Interp* ip, int objc, Tcl_Obj *CONST objv[]);
void WrapperCmdDeleteProc (ClientData clientData);

#ifndef WINAPI
#ifdef BUILD_64
typedef int (*wrapperTcl_CmdProc) (int iArgArray);
#define CALLCONV
#else
typedef int __attribute__((__stdcall__)) (*wrapperTcl_CmdProc) (int iArgArray);
#define CALLCONV __attribute__((__stdcall__))
#endif
#else 
typedef int (CALLBACK *wrapperTcl_CmdProc) (int iArgArray);
#define CALLCONV WINAPI
#endif

typedef struct CmdProc {
   wrapperTcl_CmdProc proc;
   Tcl_Interp* ip;
   char argvArrayName[65];
   int InstCnt;   
} CmdProc;

static int var_id = 0;

#ifdef __WIN32__
BOOL APIENTRY DllMain(HINSTANCE hInst, DWORD reason, LPVOID reserved)
{
      return TRUE;
}
#endif

int CALLCONV CreateCommand (Tcl_Interp* ip, char *cmdName, wrapperTcl_CmdProc proc)
{  
   CmdProc *pcmd;

   pcmd = (CmdProc *) malloc (sizeof (CmdProc));
   pcmd->proc = proc;
   pcmd->ip = ip;
   pcmd->InstCnt = 0;
   strcpy (pcmd->argvArrayName, cmdName);
   
   Tcl_CreateObjCommand (ip, cmdName, Tcl_WrapperCmdProc, (ClientData) pcmd,
                         WrapperCmdDeleteProc);   
   return TCL_OK;      
}

// May need to add a Delete proc call back also, to help keep
// things clean on the .NET side.
int Tcl_WrapperCmdProc (ClientData clientData, Tcl_Interp* ip, int objc, Tcl_Obj *CONST objv[])
{
   Tcl_Obj *ret_obj;
   CmdProc *pcmd = (CmdProc *)clientData;
   int len, iNdx = 1;
   char strNdx[12], *strValue, strParmArray[129];

   sprintf (strParmArray, "%s%-d", pcmd->argvArrayName, pcmd->InstCnt);

   sprintf (strNdx, "%d", objc);	
   Tcl_SetVar2 (ip, strParmArray, "0", strNdx,
                TCL_GLOBAL_ONLY | TCL_LEAVE_ERR_MSG);
   
   while (iNdx < objc) {
      strValue = Tcl_GetStringFromObj (objv[iNdx], &len);
      sprintf (strNdx, "%d", iNdx);	
      Tcl_SetVar2 (ip, strParmArray, strNdx, strValue,
                   TCL_GLOBAL_ONLY | TCL_LEAVE_ERR_MSG);
      iNdx++;      
   }
      
   (*pcmd->proc) (pcmd->InstCnt++);
      
   return TCL_OK;      
}

void WrapperCmdDeleteProc (ClientData clientData)
{  
   CmdProc *pcmd = (CmdProc *)clientData;
   // Add more here?
   // Maybe call a user proc to cleanup on the other side.

   free (pcmd);   
}
