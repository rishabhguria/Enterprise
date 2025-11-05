setlocal
title=Sell Side Simulator

set CLASS=com.cameronsystems.fix.oms.simulator.SimulatorSellFrame
set SIMULATOR_CLASS=simulator.class=com.cameronsystems.fix.oms.simulator.socket.SocketSimulator
  
set HOST_NAME=host.name=localhost
set PORT=port=2200

_start %CLASS% "%SIMULATOR_CLASS%" "%HOST_NAME%" "%PORT%"
