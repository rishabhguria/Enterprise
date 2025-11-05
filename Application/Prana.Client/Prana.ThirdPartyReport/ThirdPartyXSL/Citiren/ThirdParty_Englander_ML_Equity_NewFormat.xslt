<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>
        <!--for system internal use-->
        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>

        <!--for system internal use-->
        <RowHeader>
          <xsl:value-of select ="'true'"/>
        </RowHeader>

        <!--for system internal use-->
        <TaxLotState>
          <xsl:value-of select="'TaxLotState'"/>
        </TaxLotState>

        <MarketCode>
          <xsl:value-of select="'Market Code'"/>
        </MarketCode>

        <BlotterCode>
          <xsl:value-of select="'Blotter Code'"/>
        </BlotterCode>

        <Buy_Sell>
          <xsl:value-of select="'Buy Sell'"/>
        </Buy_Sell>

        <SYMBOL>
          <xsl:value-of select="'Symbol'"/>
        </SYMBOL>

        <SymbolType>
          <xsl:value-of select="'Symbol Type'"/>
        </SymbolType>

        <Qty>
          <xsl:value-of select="'Quantity'"/>
        </Qty>

        <Account>
          <xsl:value-of select="'Account Number'"/>
        </Account>

        <TraderID>
          <xsl:value-of select="'Trader ID'"/>
        </TraderID>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <exec_brkr>
          <xsl:value-of select="'Executing Brkr'"/>
        </exec_brkr>

        <contraBrkr>
          <xsl:value-of select="'Contra Brkr'"/>
        </contraBrkr>

        <exec_serv>
          <xsl:value-of select="'Execution Service'"/>
        </exec_serv>

        <ExecutionTime>
          <xsl:value-of select ="'Execution Time'"/>
        </ExecutionTime>

        <ContractCode>
          <xsl:value-of select ="'Contract Code'"/>
        </ContractCode>

        <QSR>
          <xsl:value-of select="'QSR'"/>
        </QSR>

        <BatchTypeCode>
          <xsl:value-of select="'Batch Type Code'"/>
        </BatchTypeCode>

        <trade_date>
          <xsl:value-of select ="'Trade Date'"/>
        </trade_date>

        <Settledate>
          <xsl:value-of select ="'Settle Date'"/>
        </Settledate>

        <TradeType>
          <xsl:value-of select="'Trade Type'"/>
        </TradeType>

        <Action>
          <xsl:value-of select ="'Action'"/>
        </Action>

        <CallPut>
          <xsl:value-of select="'Call Put'"/>
        </CallPut>

        <ExpDate>
          <xsl:value-of select="'Exp Date'"/>
        </ExpDate>

        <StrikePrice>
          <xsl:value-of select ="'Strike Price'"/>
        </StrikePrice>

        <principal>
          <xsl:value-of select ="'Principal Amt'"/>
        </principal>

        <interest>
          <xsl:value-of select ="'Interest Amt'"/>
        </interest>

        <OtherBrkrCommissionAmt>
          <xsl:value-of select="'Other Brkr Commission Amt'"/>
        </OtherBrkrCommissionAmt>

        <OtherBrkrCommissionRate>
          <xsl:value-of select="'Other Brkr Commission Rate'"/>
        </OtherBrkrCommissionRate>

        <CommissionRateCode>
          <xsl:value-of select="'Commission Rate Code'"/>
        </CommissionRateCode>

        <CommissionRate>
          <xsl:value-of select="'Commission Rate'"/>
        </CommissionRate>

        <ExecutingBrkrBadgeNo>
          <xsl:value-of select="'Executing Brkr Badge No'"/>
        </ExecutingBrkrBadgeNo>

        <ContraBrkrBadgeNo>
          <xsl:value-of select="'Contra Brkr Badge No'"/>
        </ContraBrkrBadgeNo>

        <OLACode>
          <xsl:value-of select="'OLA Code'"/>
        </OLACode>

        <TrailerScheme>
          <xsl:value-of select="'Trailer Scheme'"/>
        </TrailerScheme>

        <TrailerExec1>
          <xsl:value-of select="'Trailer Exec1'"/>
        </TrailerExec1>

        <TrailerExec2>
          <xsl:value-of select="'Trailer Exec2'"/>
        </TrailerExec2>

        <TrailerExec3>
          <xsl:value-of select="'Trailer Exec3'"/>
        </TrailerExec3>

        <TrailerExec4>
          <xsl:value-of select="'Trailer Exec4'"/>
        </TrailerExec4>

        <TrailerExec5>
          <xsl:value-of select="'Trailer Exec5'"/>
        </TrailerExec5>

        <TrailerExec6>
          <xsl:value-of select="'Trailer Exec6'"/>
        </TrailerExec6>

        <ExplicitFee>
          <xsl:value-of select="'Explicit Fee'"/>
        </ExplicitFee>

        <!-- system use only-->
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>
      </ThirdPartyFlatFileDetail>
      
      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>
          
          <!--for system internal use-->
          <IsCaptionChangeRequired>
            <xsl:value-of select ="'true'"/>
          </IsCaptionChangeRequired>
          
          <!--for system internal use-->
          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>
          
          <!--for system internal use-->
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <MarketCode>
            <xsl:value-of select="'E'"/>
          </MarketCode>

          <BlotterCode>
			  <xsl:choose>
				  <xsl:when test ="Asset='Equity'">
					  <xsl:value-of select="'ID'"/>
				  </xsl:when>
				  <xsl:when test ="Asset = 'EquityOption' and PutOrCall = 'PUT'">
					  <xsl:value-of select="'OP'"/>
				  </xsl:when>
				  <xsl:when test ="Asset = 'EquityOption' and PutOrCall = 'CALL'">
					  <xsl:value-of select="'OC'"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="''"/>
				  </xsl:otherwise>
			  </xsl:choose>
          </BlotterCode>

          <Buy_Sell>
            <xsl:choose>
              <xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Buy to Cover' or Side='Buy to Close'">
                <xsl:value-of select="'B'"/>
            </xsl:when>
            <xsl:when test="Side='Sell' or Side='Sell to Close'">
                <xsl:value-of select="'S'"/>
            </xsl:when>
              <xsl:when test="Side='Sell short' or Side='Sell to Open'">
                <xsl:value-of select="'3'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Cover' or Side='Buy to Close'">
                <xsl:value-of select="'7'"/>
              </xsl:when>
            <xsl:otherwise >
                <xsl:value-of select="''"/>
            </xsl:otherwise>
          </xsl:choose >
          </Buy_Sell>

          <SYMBOL>
            <xsl:choose>
            <xsl:when test ="Asset='EquityOption'">
                <xsl:value-of select="UnderlyingSymbol"/>
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="Symbol"/>
            </xsl:otherwise>
          </xsl:choose>
          </SYMBOL>

          <SymbolType>
            <xsl:value-of select="'S'"/>
          </SymbolType>
          
          <Qty>
            <xsl:value-of select="AllocatedQty"/>
          </Qty>

          <Account>
            <xsl:value-of select="AccountNo"/>
          </Account>

          <TraderID>
            <xsl:value-of select="''"/>
          </TraderID>

          <Price>
            <xsl:value-of select="AveragePrice"/>
          </Price>

          <exec_brkr>
            <xsl:choose>
            <xsl:when test ="Asset='Equity' and CounterParty='RODM'">
                <xsl:value-of select="161"/>
            </xsl:when>
            <xsl:when test ="Asset='Equity' and CounterParty ='ATLE'">
                <xsl:value-of select="443"/>
            </xsl:when>
            <xsl:when test ="Asset='Equity' and (CounterParty ='BTIG' or CounterParty ='DAP' or CounterParty ='DIPO')">
                <xsl:value-of select="501"/>
            </xsl:when>
            <xsl:when test ="Asset='Equity' and CounterParty ='LTCO'">
                <xsl:value-of select="226"/>
            </xsl:when>
				<xsl:when test ="Asset='Equity' and CounterParty ='BARC'">
					<xsl:value-of select="229"/>
				</xsl:when>
				<xsl:when test ="Asset='EquityOption' and (CounterParty ='DAP' or CounterParty ='DIPO')">
					<xsl:value-of select="501"/>
				</xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="'0'"/>
            </xsl:otherwise>
          </xsl:choose>
        </exec_brkr>

          <contraBrkr>
            <xsl:choose>
            <xsl:when test ="Asset='Equity' and CounterParty='RODM'">
                <xsl:value-of select="161"/>
            </xsl:when>
            <xsl:when test ="Asset='Equity' and CounterParty='ATLE'">
                <xsl:value-of select="443"/>
            </xsl:when>
            <xsl:when test ="Asset='Equity' and (CounterParty='BTIG' or CounterParty='DAP'  or CounterParty='DIPO')">
                <xsl:value-of select="501"/>
            </xsl:when>
            <xsl:when test ="Asset='Equity' and CounterParty='LTCO'">
                <xsl:value-of select="226"/>
            </xsl:when>
				<xsl:when test ="Asset='Equity' and CounterParty='BARC'">
					<xsl:value-of select="229"/>
				</xsl:when>
				<xsl:when test ="Asset='EquityOption' and (CounterParty='DAP' or CounterParty='DIPO')">
					<xsl:value-of select="501"/>
				</xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="'0'"/>
            </xsl:otherwise>
          </xsl:choose>
          </contraBrkr>

          <exec_serv>
            <xsl:choose>
            <xsl:when test ="Asset='Equity' or Asset = 'EquityOption'">
                <xsl:value-of select="CounterParty"/>
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="''"/>
            </xsl:otherwise>
          </xsl:choose>
          </exec_serv>

          <ExecutionTime>
            <xsl:value-of select ="''"/>
          </ExecutionTime>

          <ContractCode>
            <xsl:value-of select ="'0'"/>
          </ContractCode>

          <QSR>
            <xsl:value-of select="''"/>
          </QSR>


          <BatchTypeCode>
            <xsl:value-of select="'BO'"/>
          </BatchTypeCode>
          
          <trade_date>
            <xsl:value-of select ="translate(TradeDate,'/','.')"/>
          </trade_date>

          <Settledate>
            <xsl:value-of select ="translate(SettlementDate,'/','.')"/>
          </Settledate>

          <TradeType>
            <xsl:choose>
              <xsl:when test="Asset = 'FutureOption' or Asset = 'EquityOption'">
                <xsl:value-of select="'MO'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'MT'"/>
              </xsl:otherwise>
            </xsl:choose>
          </TradeType>


			<Action>
				<xsl:choose>
					<xsl:when test="Asset = 'FutureOption' or Asset = 'EquityOption'">
						<xsl:choose>
							<xsl:when test ="(Side = 'Buy to Open' or Side='Sell to Open' or Side='Buy' or Side='Sell short')">
								<xsl:value-of select ="'1'"/>
							</xsl:when>
							<xsl:when test ="(Side='Sell to Close' or Side = 'Buy to Close' or Side= 'Sell')">
								<xsl:value-of select ="'2'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</Action>


          <CallPut>
            <xsl:choose>
              <xsl:when test="PutOrCall != ''">
                <xsl:value-of select="substring(PutOrCall,1,1)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </CallPut>
          
          <ExpDate>
            <xsl:choose>
              <xsl:when test="ExpirationDate != '' and Asset !='Equity'">
                <xsl:value-of select="translate(ExpirationDate,'/','.')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </ExpDate>

          <StrikePrice>
			  <xsl:choose>
				  <xsl:when test="StrikePrice != '' and Asset !='Equity'">
					  <xsl:value-of select="StrikePrice"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="''"/>
				  </xsl:otherwise>
			  </xsl:choose>
          </StrikePrice>

          <principal>
            <xsl:value-of select ="''"/>
          </principal>

          <interest>
            <xsl:value-of select ="''"/>
          </interest>

          <OtherBrkrCommissionAmt>
            <xsl:value-of select="''"/>
          </OtherBrkrCommissionAmt>

          <OtherBrkrCommissionRate>
			  <xsl:choose>
				  <xsl:when test ="Asset='Equity' and CounterParty='RODM'">
					  <xsl:value-of select="0.005"/>
				  </xsl:when>
				  <xsl:when test ="Asset='Equity' and CounterParty='ATLE'">
					  <xsl:value-of select="0.03"/>
				  </xsl:when>
				  <xsl:when test ="Asset='Equity' and CounterParty='BTIG'">
					  <xsl:value-of select="0.02"/>
				  </xsl:when>
				  <xsl:when test ="Asset='Equity' and CounterParty='LTCO'">
					  <xsl:value-of select="0.02"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="'0'"/>
				  </xsl:otherwise>
			  </xsl:choose>
          </OtherBrkrCommissionRate>

          <CommissionRateCode>
            <xsl:value-of select="''"/>
          </CommissionRateCode>

          <CommissionRate>
            <xsl:value-of select="''"/>
          </CommissionRate>

          <ExecutingBrkrBadgeNo>
            <xsl:value-of select="''"/>
          </ExecutingBrkrBadgeNo>

          <ContraBrkrBadgeNo>
            <xsl:value-of select="''"/>
          </ContraBrkrBadgeNo>

          <OLACode>
            <xsl:value-of select="''"/>
          </OLACode>

          <TrailerScheme>
            <xsl:value-of select="''"/>
          </TrailerScheme>

          <TrailerExec1>
            <xsl:value-of select="''"/>
          </TrailerExec1>

          <TrailerExec2>
            <xsl:value-of select="''"/>
          </TrailerExec2>

          <TrailerExec3>
            <xsl:value-of select="''"/>
          </TrailerExec3>

          <TrailerExec4>
            <xsl:value-of select="''"/>
          </TrailerExec4>

          <TrailerExec5>
            <xsl:value-of select="''"/>
          </TrailerExec5>

          <TrailerExec6>
            <xsl:value-of select="''"/>
          </TrailerExec6>

          <ExplicitFee>
            <xsl:value-of select="''"/>
          </ExplicitFee>
          
          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
