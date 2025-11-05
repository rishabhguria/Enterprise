<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">
		public int RoundOff(double Qty)
		{
		
		return (int)Math.Round(Qty,0);
		}
	</msxsl:script>

	<xsl:template name="DateFormat">
		<xsl:param name="Date"/>
		<xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
	</xsl:template>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>
				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>


				<!--for system internal use-->
				<TaxLotState>
					<xsl:value-of select="TaxLotState"/>
				</TaxLotState>


				<IDENTIFIER>
					<xsl:value-of select="'IDENTIFIER'"/>
				</IDENTIFIER>

				<CODE>
					<xsl:value-of select="'CODE'"/>
				</CODE>

				<UNITS>
					<xsl:value-of select="'UNITS'"/>
				</UNITS>

				<PRSHR>
					<xsl:value-of select="'PRSHR'"/>
				</PRSHR>

				<BROKER>
					<xsl:value-of select="'BROKER'"/>
				</BROKER>

				<TRADDT>
					<xsl:value-of select="'TRADDT'"/>
				</TRADDT>

				<CONTDT>
					<xsl:value-of select="'CONTDT'"/>
				</CONTDT>

				<COMMS>
					<xsl:value-of select="'COMMS'"/>
				</COMMS>

				<SECFEES>
					<xsl:value-of select="'S.E.C. FEES'"/>
				</SECFEES>

				<NET>
					<xsl:value-of select="'NET'"/>
				</NET>

				<Currency>
					<xsl:value-of select="'Currency'"/>
				</Currency>

				<TICKER>
					<xsl:value-of select="'TICKER'"/>
				</TICKER>

				<SECURITYDESCRIPTION>
					<xsl:value-of select="'SECURITY DESCRIPTION'"/>
				</SECURITYDESCRIPTION>

				<TradeSettlement>
					<xsl:value-of select="'Trade Settlement'"/>
				</TradeSettlement>

				<AccountNumber>
					<xsl:value-of select="'Account Number:'"/>
				</AccountNumber>

					<TradeStatus>
					<xsl:value-of select="'TradeStatus'"/>
					</TradeStatus>
					
					<TransactionNumber>
					<xsl:value-of select="'TransactionNumber'"/>
					</TransactionNumber>
        
        <!--swap-->

        <AssetType>
          <xsl:value-of select="'Asset Type'"/>
        </AssetType>

        <InvestmentType>
          <xsl:value-of select="'Investment Type'"/>
        </InvestmentType>

        <InvestmentDescription>
          <xsl:value-of select="'Investment Description'"/>
        </InvestmentDescription>

        <FirstResetDate>
          <xsl:value-of select="'First Reset Date'"/>
        </FirstResetDate>

        <SwapExpireDate>
          <xsl:value-of select="'Swap Expire Date'"/>
        </SwapExpireDate>

        <UnderlyingInvestment>
          <xsl:value-of select="'Underlying Investment'"/>
        </UnderlyingInvestment>

        <FinancingIssueDate>
          <xsl:value-of select="'Financing Issue Date'"/>
        </FinancingIssueDate>

        <FinancingMaturityDate>
          <xsl:value-of select="'Financing Maturity Date'"/>
        </FinancingMaturityDate>

        <FinancingAccrual>
          <xsl:value-of select="'Financing Accrual Days/Month'"/>
        </FinancingAccrual>
        
        <FinancingAccrual1>
          <xsl:value-of select="'Financing Accrual Days/Year'"/>
        </FinancingAccrual1>

        <FinancingRateResetFrequency>
          <xsl:value-of select="'Financing Rate Reset Frequency'"/>
        </FinancingRateResetFrequency>
        
        <FinancingReferenceIndex>
          <xsl:value-of select="'Financing Reference Index'"/>
        </FinancingReferenceIndex>
        
        <FinancingRateSpread>
          <xsl:value-of select="'Financing Rate Spread'"/>
        </FinancingRateSpread>


        <EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>
					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>


					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>
					<IDENTIFIER>
						<xsl:choose>
							<xsl:when test="CUSIP!='' and CurrencySymbol= 'USD'">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="SEDOL"/>
							</xsl:otherwise>
						</xsl:choose>
					</IDENTIFIER>

			<CODE>
					
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:choose>
                  <xsl:when test="Side='Buy to Open'">
								<xsl:value-of select="'Buy to Open'"/>
							</xsl:when>
							<xsl:when test="Side='Sell'">
								<xsl:value-of select="'SELL'"/>
							</xsl:when>
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select="'Buy to Close'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell to Open'">
                    <xsl:value-of select="'Sell to Open'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell to Close'">
                    <xsl:value-of select="'Sell to Close'"/>
                  </xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:choose>
                  <xsl:when test="Side='Buy'">
                    <xsl:value-of select="'BUY'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell'">
                    <xsl:value-of select="'SELL'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell short'">
                    <xsl:value-of select="'Sell Short'"/>
                  </xsl:when>
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select="'Buy to Close'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:otherwise>
            </xsl:choose>

					</CODE>

					<xsl:variable name ="Qty">
						<xsl:value-of select="my:RoundOff(AllocatedQty)"/>
					</xsl:variable>

					<UNITS>
						<xsl:choose>
							<xsl:when test="number($Qty)">
								<xsl:value-of select="$Qty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</UNITS>

					<PRSHR>
						<xsl:choose>
							<xsl:when test="number(AveragePrice)">
								<xsl:value-of select="format-number(AveragePrice,'0.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</PRSHR>

					<xsl:variable name="PB_NAME" select="'SEI'"/>

					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
					</xsl:variable>

					<xsl:variable name="Broker">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					
          <xsl:variable name="THIRDPARTY_BROKER">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBrokerCode=$PRANA_COUNTERPARTY_NAME]/@PBBroker"/>
          </xsl:variable>

					<BROKER>
						<xsl:choose>
              <xsl:when test="$THIRDPARTY_BROKER!= ''">
                <xsl:value-of select="$THIRDPARTY_BROKER"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
					</BROKER>

					<TRADDT>
						<xsl:value-of select="TradeDate"/>
					</TRADDT>

					<CONTDT>					
						<xsl:value-of select="SettlementDate"/>
					</CONTDT>

					<xsl:variable name="Commission1">
						<xsl:value-of select="SoftCommissionCharged + CommissionCharged"/>
					</xsl:variable>

					<COMMS>
						<xsl:choose>
							<xsl:when test="number($Commission1)">
								<xsl:value-of select="format-number($Commission1,'0.##')"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</COMMS>

					<SECFEES>
						<xsl:value-of select="format-number(SecFee,'0.##')"/>
					</SECFEES>
          
					<NET>
						<xsl:choose>
							<xsl:when test="number(NetAmount)">
								<xsl:value-of select="format-number(NetAmount,'0.##')"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</NET>

					<Currency>
						<xsl:value-of select="CurrencySymbol"/>
					</Currency>

					<TICKER>
            <xsl:choose>
              <xsl:when test="Asset='Equity' or IsSwapped='true'">
                <xsl:value-of select="Symbol"/>
              </xsl:when>

              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="OSIOptionSymbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
					</TICKER>

					<SECURITYDESCRIPTION>
						<xsl:value-of select="FullSecurityName"/>
					</SECURITYDESCRIPTION>
					
					<xsl:variable name="varCurrencySymbol">
						<xsl:value-of select="CurrencySymbol"/>
					</xsl:variable>

					<xsl:variable name ="varDTCCode">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_BrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$varCurrencySymbol]/@DTCCode"/>
					</xsl:variable>

					<TradeSettlement>
						<xsl:choose>
							<xsl:when test ="$varDTCCode!=''">
								<xsl:value-of select ="$varDTCCode"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</TradeSettlement>

					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="PB_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PRANA_FUND_NAME]/@PranaFund"/>
					</xsl:variable>

					<AccountNumber>
						<xsl:choose>
							<xsl:when test="$PB_FUND_CODE!=''">
								<xsl:value-of select="$PB_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="AccountNo"/>
							</xsl:otherwise>
						</xsl:choose>
					</AccountNumber>

			
					<xsl:variable name="varTaxlotState">
						<xsl:choose>
						<xsl:when test="TaxLotState='Allocated'">
							<xsl:value-of select ="'NEW'"/>
						</xsl:when>
						<xsl:when test="TaxLotState='Amended'">
							<xsl:value-of select ="'COR'"/>
						</xsl:when>
						<xsl:when test="TaxLotState='Deleted'">
							<xsl:value-of select ="'CAN'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="''"/>
						</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					<TradeStatus>
					<xsl:value-of select="$varTaxlotState"/>
					</TradeStatus>
					
					<TransactionNumber>
					<xsl:value-of select="EntityID"/>
					</TransactionNumber>
					

          <!--swap-->

          <AssetType>
            <xsl:choose>
              <xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="'SWAP'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </AssetType>

          <InvestmentType>
            <xsl:choose>
              <xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="'TRS'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </InvestmentType>

          <xsl:variable name="varGetBeforeEquity">
                <xsl:value-of select ="substring-before(BBCode,'Equity')"/>          
          </xsl:variable>
          <xsl:variable name="varGetafterEquity">
            <xsl:value-of select ="concat($varGetBeforeEquity,'TRS',' ','EQUITY',' ',TradeAttribute6)"/>
          </xsl:variable>
          
          <InvestmentDescription>
            <xsl:choose>
              <xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="$varGetafterEquity"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </InvestmentDescription>

          <FirstResetDate>
            <xsl:value-of select="TradeAttribute1"/>
          </FirstResetDate>

          <SwapExpireDate>
            <xsl:value-of select="TradeAttribute2"/>
          </SwapExpireDate>

          <UnderlyingInvestment>
            <xsl:choose>
              <xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="BBCode"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>          
          </UnderlyingInvestment>

          <FinancingIssueDate>
            <xsl:choose>
              <xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="TradeAttribute3"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>          
          </FinancingIssueDate>

          <FinancingMaturityDate>
            <xsl:choose>
              <xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="TradeAttribute4"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </FinancingMaturityDate>

          <FinancingAccrual>
            <xsl:choose>
              <xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="substring-before(TradeAttribute5,' ')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>        
          </FinancingAccrual>
          
          <FinancingAccrual1>
            <xsl:choose>
              <xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="substring-before(substring-after(TradeAttribute5,' '),' ')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>           
          </FinancingAccrual1>

          <FinancingRateResetFrequency>
            <xsl:choose>
              <xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="substring-after(substring-after(TradeAttribute5,' '),' ')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
            
          </FinancingRateResetFrequency>

          <FinancingReferenceIndex>
            <xsl:value-of select="''"/>
          </FinancingReferenceIndex>

          <FinancingRateSpread>
            <xsl:choose>
              <xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="TradeAttribute6"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
            
          </FinancingRateSpread>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
