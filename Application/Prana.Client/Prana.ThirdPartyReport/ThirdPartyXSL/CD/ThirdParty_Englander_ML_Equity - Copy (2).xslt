<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>
          <!--for system internal use-->
          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>
          <!--for system internal use-->
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>


			<xsl:choose>
				<xsl:when test ="Asset='Equity'">
					<mkt_blt>
						<xsl:value-of select="'EID'"/>
					</mkt_blt>
				</xsl:when>
				<xsl:when test ="Asset = 'EquityOption' and PutOrCall = 'PUT'">
					<mkt_blt>
						<xsl:value-of select="'EOP'"/>
					</mkt_blt>
				</xsl:when>
				<xsl:when test ="Asset = 'EquityOption' and PutOrCall = 'CALL'">
					<mkt_blt>
						<xsl:value-of select="'EOC'"/>
					</mkt_blt>
				</xsl:when>
				<xsl:otherwise>
					<mkt_blt>
						<xsl:value-of select="''"/>
					</mkt_blt>				
			</xsl:otherwise>
			</xsl:choose>
			
			<!--<xsl:choose>
				<xsl:when test ="Asset='Equity'">
					<mkt_blt>
						<xsl:value-of select="'EID'"/>
					</mkt_blt>
						</xsl:when>
				<xsl:otherwise>
					<mkt_blt>
						<xsl:value-of select="''"/>
					</mkt_blt>
						</xsl:otherwise>
			</xsl:choose>-->
			
			<!--<mkt_blt>
            <xsl:value-of select ="'EID'"/>
          </mkt_blt>-->

          <!-- Side Starts-->
          <xsl:choose>
            <xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Buy to Cover' or Side='Buy to Close'">
              <Buy_Sell>
                <xsl:value-of select="'B'"/>
              </Buy_Sell>
            </xsl:when>
            <xsl:when test="Side='Sell' or Side='Sell to Close' or Side='Sell short' or Side='Sell to Open'">
              <Buy_Sell>
                <xsl:value-of select="'S'"/>
              </Buy_Sell>
            </xsl:when>
            <xsl:otherwise >
              <Buy_Sell>
                <xsl:value-of select="''"/>
              </Buy_Sell>
            </xsl:otherwise>
          </xsl:choose >

          <xsl:choose>
            <!--<xsl:when test="Side='Buy to Cover' or Side='Buy to Close'">
              <Short>
                <xsl:value-of select ="''"/>
              </Short>
            </xsl:when>-->
            <xsl:when test="Side='Sell short' or Side='Sell to Open'">
              <Short>
                <xsl:value-of select ="'S'"/>
              </Short>
            </xsl:when>
            <xsl:otherwise>
              <Short>
                <xsl:value-of select ="''"/>
              </Short>
            </xsl:otherwise>
          </xsl:choose >

			<xsl:choose>
				<xsl:when test ="Asset='EquityOption'">
					<!--<SYMBOL>
						<xsl:value-of select="substring-before(Symbol,' ')"/>
					</SYMBOL>-->
					<SYMBOL>
						<xsl:value-of select="UnderlyingSymbol"/>
					</SYMBOL>
				</xsl:when>
				<xsl:otherwise>
					<SYMBOL>
						<xsl:value-of select="Symbol"/>
					</SYMBOL>
				</xsl:otherwise>
			</xsl:choose>

          <Qty>
            <xsl:value-of select="AllocatedQty"/>
          </Qty>

          <Account>
            <xsl:value-of select="FundAccountNo"/>
          </Account>

          <Price>
            <xsl:value-of select="AveragePrice"/>
          </Price>

			<xsl:choose>
				<xsl:when test ="Asset='Equity' and CounterParty='RODM'">
					<exec_brkr>
						<xsl:value-of select="'161'"/>
					</exec_brkr>
				</xsl:when>
				<xsl:when test ="Asset='Equity' and CounterParty !='RODM'">
					<exec_brkr>
						<xsl:value-of select="CounterParty"/>
					</exec_brkr>
				</xsl:when>
				<xsl:otherwise>
					<exec_brkr>
						<xsl:value-of select="'0'"/>
					</exec_brkr>
				</xsl:otherwise>
			</xsl:choose>
			
			
		  <!--<exec_brkr>
            <xsl:value-of select ="CounterParty"/>
          </exec_brkr>-->

			<xsl:choose>
				<xsl:when test ="Asset='Equity'">
					<rec_del>
						<xsl:value-of select="161"/>
					</rec_del>
						</xsl:when>
				<xsl:otherwise>
					<rec_del>
						<xsl:value-of select="'0'"/>
					</rec_del>
				</xsl:otherwise>
			</xsl:choose>


			<!--<rec_del>
            <xsl:value-of select ="CounterParty"/>
          </rec_del>-->


			<xsl:choose>
				<xsl:when test ="Asset='Equity'">
					<exec_serv>
						<xsl:value-of select="CounterParty"/>
					</exec_serv>
				</xsl:when>
				<xsl:when test ="Asset = 'EquityOption'">
					<exec_serv>
						<xsl:value-of select="CounterParty"/>
					</exec_serv>
				</xsl:when>
				<xsl:otherwise>
					<exec_serv>
						<xsl:value-of select="''"/>
					</exec_serv>
				</xsl:otherwise>
			</xsl:choose>
			
			<!--<xsl:choose>
				<xsl:when test ="Asset='Equity'">
					<exec_serv>
						<xsl:value-of select="'GLPB'"/>
					</exec_serv>
				</xsl:when>
				<xsl:when test ="Asset = 'EquityOption'">
					<exec_serv>
						<xsl:value-of select="'CMTA'"/>
					</exec_serv>
				<xsl:otherwise>
					<exec_serv>
						<xsl:value-of select="''"/>
					</exec_serv>
				</xsl:otherwise>
			</xsl:choose>-->
			
			
			<!--<exec_serv>
            <xsl:value-of select="''"/>
          </exec_serv>-->

          <time>
            <xsl:value-of select ="''"/>
          </time>

          <cntrc>
            <xsl:value-of select ="''"/>
          </cntrc>

          <client_id>
            <xsl:value-of select ="''"/>
          </client_id>

          <hse_both>
            <xsl:value-of select ="''"/>
          </hse_both>

          <trade_date>
            <xsl:value-of select ="TradeDate"/>
          </trade_date>

          <set_date>
            <xsl:value-of select ="''"/>
          </set_date>

          <MISC05>
            <xsl:value-of select ="''"/>
          </MISC05>

			<xsl:choose>
				<xsl:when test ="Asset='EquityOption'">
					<Sec_mjr>
						<xsl:value-of select="'O'"/>
					</Sec_mjr>
						</xsl:when>
				<xsl:otherwise>
					<Sec_mjr>
						<xsl:value-of select="''"/>
					</Sec_mjr>
				</xsl:otherwise>
			</xsl:choose>
			
			
			
			<!--<Sec_mjr>
            <xsl:value-of select ="'O'"/>
          </Sec_mjr>-->

          <xsl:choose>
            <xsl:when test ="Asset = 'EquityOption' and (Side = 'Buy to Open' or Side='Sell to Open')">
              <OPTN_ACTN>
                <xsl:value-of select ="'O'"/>
              </OPTN_ACTN>
            </xsl:when>
            <xsl:when test ="Asset = 'EquityOption' and (Side='Sell to Close' or Side = 'Buy to Close')">
              <OPTN_ACTN>
                <xsl:value-of select ="'X'"/>
              </OPTN_ACTN>
            </xsl:when>
            <xsl:otherwise>
              <OPTN_ACTN>
                <xsl:value-of select ="''"/>
              </OPTN_ACTN>
            </xsl:otherwise>
          </xsl:choose>

          <TRAILER>
            <xsl:value-of select ="''"/>
          </TRAILER>

          <xsl:variable name = "varExpMth" >
            <xsl:value-of select="substring(ExpirationDate,1,2)"/>
          </xsl:variable>
          <xsl:variable name = "varExpYR" >
            <xsl:value-of select="substring(ExpirationDate,9,2)"/>
          </xsl:variable>
			<xsl:variable name = "varExpDAY" >
				<xsl:value-of select="substring(ExpirationDate,4,2)"/>
			</xsl:variable>
          <xsl:choose>
            <xsl:when test ="Asset='EquityOption'">
              <EXP_YR>
                <xsl:value-of select="$varExpYR"/>
              </EXP_YR>
              <EXP_MO>
                <xsl:value-of select="$varExpMth"/>
              </EXP_MO>
			  <EXP_DAY>
					<xsl:value-of select="$varExpDAY"/>
			  </EXP_DAY>
            </xsl:when>
            <xsl:otherwise>
              <EXP_YR>
                <xsl:value-of select ="''"/>
              </EXP_YR>
              <EXP_MO>
                <xsl:value-of select ="''"/>
              </EXP_MO>
				<EXP_DAY>
					<xsl:value-of select ="''"/>
				</EXP_DAY>
            </xsl:otherwise>
          </xsl:choose>

          <xsl:choose>
            <xsl:when test="Asset='EquityOption'">
				<xsl:variable name="varPriceBefore">
					<xsl:value-of select ="substring-before(StrikePrice,'.')"/>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test ="$varPriceBefore != ''">
						<STRIKE_WHOLE>
							<xsl:value-of select ="$varPriceBefore"/>
						</STRIKE_WHOLE>
					</xsl:when>
					<xsl:when test ="$varPriceBefore = ''">
						<STRIKE_WHOLE>
							<xsl:value-of select ="StrikePrice"/>
						</STRIKE_WHOLE>
					</xsl:when>
					<xsl:otherwise>
						<STRIKE_WHOLE>
							<xsl:value-of select ="StrikePrice"/>
						</STRIKE_WHOLE>
					</xsl:otherwise>
				</xsl:choose>                
            </xsl:when>
            <xsl:otherwise>
              <STRIKE_WHOLE>
                <xsl:value-of select ="''"/>
              </STRIKE_WHOLE>
            </xsl:otherwise>
          </xsl:choose >


			<xsl:choose>
				<xsl:when test="Asset='EquityOption'">
					<xsl:variable name="varPriceAfter">
						<xsl:value-of select ="substring-after(StrikePrice,'.')"/>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test ="$varPriceAfter != ''">
							<STRIKE_FRAC>
								<xsl:value-of select ="$varPriceAfter"/>
							</STRIKE_FRAC>
						</xsl:when>
						<xsl:otherwise>
							<STRIKE_FRAC>
								<xsl:value-of select ="0"/>
							</STRIKE_FRAC>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
				<xsl:otherwise>
					<STRIKE_FRAC>
						<xsl:value-of select ="''"/>
					</STRIKE_FRAC>
				</xsl:otherwise>
			</xsl:choose >


          <principal>
            <xsl:value-of select ="''"/>
          </principal>

          <interest>
            <xsl:value-of select ="''"/>
          </interest>

          <Whole_Dollar_Away_Commission>
            <xsl:value-of select ="''"/>
          </Whole_Dollar_Away_Commission>

          <Introduced_per_share>
            <xsl:value-of select ="''"/>
          </Introduced_per_share>

          <Introduced_Whole_Dollar>
            <xsl:value-of select ="''"/>
          </Introduced_Whole_Dollar>

          <EX_BADGE>
            <xsl:value-of select ="''"/>
          </EX_BADGE>

          <RD_BADGE>
            <xsl:value-of select ="''"/>
          </RD_BADGE>

          <FLIP>
            <xsl:value-of select ="''"/>
          </FLIP>

          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
