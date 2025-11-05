<?xml version="1.0" encoding="UTF-8"?>
								<!-- Description: MSFS_Sensato EOD file, Created Date: 02-13-2012(mm-DD-YY)-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="noofzeros">
		<xsl:param name="count"/>
		<xsl:if test="$count > 0">
			<xsl:value-of select ="'0'"/>
			<xsl:call-template name="noofzeros">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="noofBlanks">
		<xsl:param name="count1"/>
		<xsl:if test="$count1 > 0">
			<xsl:value-of select ="' '"/>
			<xsl:call-template name="noofBlanks">
				<xsl:with-param name="count1" select="$count1 - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<ThirdPartyFlatFileDetail>
				<!--for system internal use-->
				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<!--for system use only-->
				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<!--for system internal use-->
				<TaxLotState>
					<xsl:value-of select ="'TaxLotState'"/>
				</TaxLotState>

				<transType>
					<xsl:value-of select="'transType'"/>
				</transType>

				<TransStatus>
					<xsl:value-of select="'TransStatus'"/>
				</TransStatus>

				<BuySell>
					<xsl:value-of select="'Buy/Sell'"/>
				</BuySell>

				<LongShot>
					<xsl:value-of select="'Long/Shot'"/>
				</LongShot>

				<PosType>
					<xsl:value-of select="'PosType'"/>
				</PosType>

				<translevel>
					<xsl:value-of select="'trans level'"/>
				</translevel>

				<ClientRef>
					<xsl:value-of select="'Client Ref#'"/>
				</ClientRef>

				<Associated>
					<xsl:value-of select="'Associated #'"/>
				</Associated>

				<ExecAccount>
					<xsl:value-of select="'Exec Account'"/>
				</ExecAccount>

				<CustAccount>
					<xsl:value-of select="'Cust Account'"/>
				</CustAccount>

				<ExecBkr>
					<xsl:value-of select="'Exec Bkr'"/>
				</ExecBkr>

				<SecType>
					<xsl:value-of select="'Sec Type'"/>
				</SecType>

				<SecID>
					<xsl:value-of select="'Sec ID'"/>
				</SecID>

				<desc>
					<xsl:value-of select="'desc'"/>
				</desc>

				<Tdate>
					<xsl:value-of select="'Tdate'"/>
				</Tdate>

				<Sdate>
					<xsl:value-of select="'Sdate'"/>
				</Sdate>

				<CCY>
					<xsl:value-of select="'CCY'"/>
				</CCY>

				<ExCode>
					<xsl:value-of select="'Ex Code'"/>
				</ExCode>

				<qty>
					<xsl:value-of select="'qty'"/>
				</qty>

				<price>
					<xsl:value-of select="'price'"/>
				</price>

				<type>
					<xsl:value-of select="'type'"/>
				</type>

				<prin>
					<xsl:value-of select="'prin'"/>
				</prin>

				<comm>
					<xsl:value-of select="'comm'"/>
				</comm>

				<comtype>
					<xsl:value-of select="'com type'"/>
				</comtype>

				<Othercharges>
					<xsl:value-of select="'Other charges'"/>
				</Othercharges>

				<Taxfees>
					<xsl:value-of select="'Tax fees'"/>
				</Taxfees>

				<feesind>
					<xsl:value-of select="'fees ind'"/>
				</feesind>

				<interest>
					<xsl:value-of select="'interest'"/>
				</interest>


				<InterestIndicator>
					<xsl:value-of select="'Interest Indicator'"/>
				</InterestIndicator>

				<netamount>
					<xsl:value-of select="'net amount'"/>
				</netamount>

				<hsyind>
					<xsl:value-of select="'hsy ind'"/>
				</hsyind>

				<custbkr>
					<xsl:value-of select="'cust bkr'"/>
				</custbkr>

				<mmgr>
					<xsl:value-of select="'mmgr'"/>
				</mmgr>

				<bookid>
					<xsl:value-of select="'book id'"/>
				</bookid>

				<dealid>
					<xsl:value-of select="'deal id'"/>
				</dealid>


				<taxlotid>
					<xsl:value-of select="'taxlot id'"/>
				</taxlotid>


				<taxdate>
					<xsl:value-of select="'tax date'"/>
				</taxdate>

				<taxprice>
					<xsl:value-of select="'tax price'"/>
				</taxprice>


				<closeoutmethod>
					<xsl:value-of select="'closeout method'"/>
				</closeoutmethod>

				<exrate>
					<xsl:value-of select="'ex rate'"/>
				</exrate>

				<acqdate>
					<xsl:value-of select="'acq date'"/>
				</acqdate>

				<instx>
					<xsl:value-of select="'instx'"/>
				</instx>
				
				<!-- system use only-->
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>
			</ThirdPartyFlatFileDetail>

			<!--<xsl:for-each select="ThirdPartyFlatFileDetail[(Asset !='FX' and Asset !='FXForward') and (FundName = '73Y1F0' or FundName = 'PMFG' or FundName = '10607884' or FundName = '038CDFGT3' or FundName = '002-200640')]">-->
			<xsl:for-each select="ThirdPartyFlatFileDetail[(Asset !='FX' and Asset !='FXForward') and (AccountName = 'International Account' or AccountName = 'SP29CS_CASH' or AccountName = 'SP29DB_CASH' or AccountName = '10607884' or AccountName = 'SP29MS_ALL' or AccountName = 'SP29GS_CASH')]">
				<ThirdPartyFlatFileDetail>
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="true"/>
					</RowHeader>
					
					<!--for system use only-->					
					<IsCaptionChangeRequired>
						<xsl:value-of select ="true"/>
					</IsCaptionChangeRequired>
					
					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
					<xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>
		
					<transType>
						<xsl:value-of select="'TR001'"/>
					</transType>
					
					<xsl:variable name="varTransStatus">
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select="'NEW'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select="'CAN'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amemded'">
								<xsl:value-of select="'COR'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'NEW'"/>
							</xsl:otherwise>
						</xsl:choose>	
					</xsl:variable>
					
					
					<TransStatus>
						<xsl:value-of select="$varTransStatus"/>
					</TransStatus>

					<!--Optional Field as spec-->
						
					<BuySell>
						<xsl:value-of select="''"/>
					</BuySell>
					
					<!--Optional Field as spec-->
					<LongShot>
						<xsl:value-of select="''"/>
					</LongShot>				

					<xsl:variable name="varPosType">
						<xsl:choose>
							<xsl:when test="Side='Buy to Open' or Side='Buy'">
								<xsl:value-of select="'BL'"/>
							</xsl:when>
							<xsl:when test=" Side='Sell' or Side='Sell to Close'">
								<xsl:value-of select="'SL'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'SS'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'BC'"/>
							</xsl:when>							
						</xsl:choose>
					</xsl:variable>
					
					<PosType>
						<xsl:value-of select="$varPosType"/>
					</PosType>
					
					<translevel>
						<xsl:value-of select="'S'"/>
					</translevel>

					<ClientRef>
						<xsl:value-of select="TradeRefID"/>
					</ClientRef>
										
					<Associated>
						<xsl:value-of select="''"/>
					</Associated>

					<ExecAccount>
						<xsl:value-of select="''"/>
					</ExecAccount>

					<CustAccount>
						<!--<xsl:value-of select="AccountNo"/>-->
						<xsl:value-of select="'038Q5053'"/>
					</CustAccount>

					<xsl:variable name="varCounterParty">
						<xsl:if test="CounterParty != ''">
							<xsl:value-of select="CounterParty"/>
						</xsl:if>
					</xsl:variable>

					<xsl:variable name="varExecutionBroker">
						<xsl:if test="$varCounterParty != ''">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='CITCO']/BrokerData[@PranaBroker = $varCounterParty]/@PBBroker"/>
						</xsl:if>
					</xsl:variable>

					<ExecBkr>
						<xsl:choose>
							<xsl:when test="$varExecutionBroker != ''">
								<xsl:value-of select ="$varExecutionBroker"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="CounterParty"/>
							</xsl:otherwise>
						</xsl:choose>

					</ExecBkr>



					<xsl:choose>
						<xsl:when test ="Asset='Equity'">
							<SecType>
								<xsl:value-of select="'S'"/>
							</SecType>
						</xsl:when>
						<!--<xsl:when test ="Asset='EquityOption'">
							<SecType>
								<xsl:value-of select="'OCC'"/>
							</SecType>
						</xsl:when>-->
						<xsl:otherwise>
							<SecType>
								<xsl:value-of select="'T'"/>
							</SecType>
						</xsl:otherwise>
					</xsl:choose>


					<!-- For Equity Option OSI Symbology-->

					<xsl:variable name="varOptionUnderlying">
						<xsl:value-of select="substring-after(substring-before(Symbol,' '),':')"/>
					</xsl:variable>

					<xsl:variable name = "BlankCount_Root" >
						<xsl:call-template name="noofBlanks">
							<xsl:with-param name="count1" select="(6) - string-length($varOptionUnderlying)" />
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="varFormattedStrikePrice">
						<xsl:value-of select="format-number(StrikePrice,'00000.000')"/>
					</xsl:variable>

					<xsl:variable name="varOSIOptionSymbol">
						<xsl:value-of select="concat($varOptionUnderlying,$BlankCount_Root,substring(ExpirationDate,9,2),substring(ExpirationDate,1,2),substring(ExpirationDate,4,2),substring(PutOrCall,1,1),translate($varFormattedStrikePrice,'.',''))"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test ="Asset='Equity'">
							<SecID>
								<xsl:value-of select="SEDOL"/>
							</SecID>
						</xsl:when>
						<xsl:when test ="Asset='EquityOption'">
							<SecID>
								<xsl:choose>
									<xsl:when test="OSIOptionSymbol != ''">
										<xsl:value-of select="translate(OSIOptionSymbol,' ','')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="translate($varOSIOptionSymbol,' ','')"/>
									</xsl:otherwise>
								</xsl:choose>
							</SecID>
						</xsl:when>
						<xsl:otherwise>
							<SecID>
								<xsl:value-of select="Symbol"/>
							</SecID>
						</xsl:otherwise>
					</xsl:choose>
					
					<!--<SecID>
						<xsl:value-of select="SEDOL"/>
					</SecID>-->

					<desc>
						<xsl:value-of select="concat(FullSecurityName,'')"/>
					</desc>

					<!--MMddyyyy-->
					<Tdate>
						<!--<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>-->
						<xsl:value-of select="TradeDate"/>
					</Tdate>

					<Sdate>
						<!--<xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>-->
						<xsl:value-of select="SettlementDate"/>
					</Sdate>

					<CCY>
						<xsl:value-of select="CurrencySymbol"/>
					</CCY>					
					
					<ExCode>
						<xsl:value-of select="''"/>
					</ExCode>

					<qty>
						<xsl:value-of select="AllocatedQty"/>
					</qty>

					<!--<price>
						<xsl:value-of select="AveragePrice"/>
					</price>-->


					<!--<xsl:choose>
						<xsl:when test ="FundName = '10607884'">
							<price>
								<xsl:value-of select='format-number(AveragePrice, "####.00")'/>
							</price>
						</xsl:when>
						<xsl:otherwise>
							<price>
								<xsl:value-of select="AveragePrice"/>
							</price>
						</xsl:otherwise>
					</xsl:choose>-->

					<xsl:choose>
						<xsl:when test ="AccountName = '10607884'">
							<price>
								<xsl:value-of select='format-number(AveragePrice, "####.00")'/>
							</price>
						</xsl:when>
						<!-- GS Funds Average price will be upto 4 decimal places-->
						<xsl:when test="AccountNo = '002200079' or AccountNo = '002200640' or AccountNo = '002276293' or AccountNo = '002-439826' or AccountNo = '013101969'or AccountNo = '013216619' or AccountNo = '013425764'">
							<price>
								<xsl:value-of select='format-number(AveragePrice, "0.0000")'/>
							</price>
						</xsl:when>
						<xsl:otherwise>
							<price>
								<xsl:value-of select="AveragePrice"/>
							</price>
						</xsl:otherwise>
					</xsl:choose>					

					<type>
						<xsl:value-of select="'G'"/>
					</type>
										
					<prin>
						<xsl:value-of select='format-number(GrossAmount, "###.00")'/>
						<!--<xsl:value-of select="GrossAmount"/>-->
					</prin>

					<comm>
						<xsl:value-of select='format-number(CommissionCharged, "###.00")'/>
						<!--<xsl:value-of select="CommissionCharged"/>-->
					</comm>
			
					<comtype>
						<xsl:value-of select="'F'"/>
					</comtype>

					<Othercharges>
						<!--<xsl:value-of select="0"/>-->
						<xsl:value-of select='format-number(StampDuty + TaxOnCommissions + ClearingFee + OtherBrokerFee + MiscFees + TransactionLevy, "###.00")'/>
					</Othercharges>

					<Taxfees>
						<xsl:value-of select="0"/>
					</Taxfees>

					<feesind>
						<xsl:value-of select="'F'"/>
					</feesind>

					<interest>
						<xsl:value-of select="0"/>
					</interest>
				
					<InterestIndicator>
						<xsl:value-of select="'F'"/>
					</InterestIndicator>
			
					<netamount>
						<xsl:value-of select='format-number(NetAmount, "###.00")'/>
						<!--<xsl:value-of select="NetAmount"/>-->
					</netamount>

					<hsyind>
						<xsl:value-of select="'Y'"/>
					</hsyind>

					<!--<xsl:choose>
						<xsl:when test ="FundName = '73Y1F0'">
							<custbkr>
								<xsl:value-of select="'CSFB'"/>
							</custbkr>
						</xsl:when>
						<xsl:when test ="FundName = 'PMFG' or FundName = '10607884'">
							<custbkr>
								<xsl:value-of select="'DBAB'"/>
							</custbkr>
						</xsl:when>
						<xsl:when test ="FundName = '002-200640'">
							<custbkr>
								<xsl:value-of select="'GSCO'"/>
							</custbkr>
						</xsl:when>
						<xsl:when test ="FundName = '038CDFGT3'">
							<custbkr>
								<xsl:value-of select="'MSCO'"/>
							</custbkr>
						</xsl:when>
						<xsl:otherwise>
							<custbkr>
								<xsl:value-of select="CounterParty"/>
							</custbkr>
						</xsl:otherwise>
					</xsl:choose>-->


					<xsl:variable name="varFundName">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name="varPB">
						<xsl:value-of select="document('../ReconMappingXml/FundwisePBMapping.xml')/BrokerMapping/PB[@Name='SENSATO']/BrokerData[@PranaFundName = $varFundName]/@PB"/>
					</xsl:variable>

					<custbkr>
						<xsl:choose>
							<xsl:when test="$varPB != ''">
								<xsl:value-of select ="$varPB"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</custbkr>

					<mmgr>
						<xsl:value-of select="''"/>
					</mmgr>

					<bookid>
						<xsl:value-of select="''"/>
					</bookid>

					<dealid>
						<xsl:value-of select="''"/>
					</dealid>

					<taxlotid>
						<xsl:value-of select="''"/>
					</taxlotid>

					<taxdate>
						<xsl:value-of select="''"/>
					</taxdate>

					<taxprice>
						<xsl:value-of select="''"/>
					</taxprice>

					<closeoutmethod>
						<xsl:value-of select="''"/>
					</closeoutmethod>

					<exrate>
						<xsl:value-of select="''"/>
					</exrate>

					<acqdate>
						<xsl:value-of select="''"/>
					</acqdate>

					<instx>
						<xsl:value-of select="''"/>
					</instx>

					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
