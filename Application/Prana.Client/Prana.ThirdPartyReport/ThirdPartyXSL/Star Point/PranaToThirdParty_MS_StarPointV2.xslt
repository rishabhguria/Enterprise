<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<transType>
						<xsl:value-of select="'TR001'"/>
					</transType>

					<xsl:choose>
						<xsl:when test ="TaxLotState = 'Allocated'">
							<TransStatus>
								<xsl:value-of select="'NEW'"/>
							</TransStatus>
						</xsl:when>
						<xsl:when test ="TaxLotState = 'Amemded'">
							<TransStatus>
								<xsl:value-of select="'COR'"/>
							</TransStatus>
						</xsl:when>
						<xsl:when test ="TaxLotState = 'Deleted'">
							<TransStatus>
								<xsl:value-of select="'CAN'"/>
							</TransStatus>
						</xsl:when>
						<xsl:otherwise>
							<TransStatus>
								<xsl:value-of select="'NEW'"/>
							</TransStatus>
						</xsl:otherwise>
					</xsl:choose>

					<BuySell>
						<xsl:value-of select="''"/>
					</BuySell>

					<LongShot>
						<xsl:value-of select="''"/>
					</LongShot>

					<!--   Side     -->

					<xsl:choose>
						<xsl:when test="Side='Buy' or Side='Buy to Open'">
							<PosType>
								<xsl:value-of select="'BL'"/>
							</PosType>
						</xsl:when>
						<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
							<PosType>
								<xsl:value-of select="'BC'"/>
							</PosType>
						</xsl:when>
						<xsl:when test="Side='Sell' or Side='Sell to Close'">
							<PosType>
								<xsl:value-of select="'SL'"/>
							</PosType>
						</xsl:when>
						<xsl:when test="Side='Sell short' or Side='Sell to Open'">
							<PosType>
								<xsl:value-of select="'SS'"/>
							</PosType>
						</xsl:when>
						<xsl:otherwise>
							<PosType>
								<xsl:value-of select="''"/>
							</PosType>
						</xsl:otherwise>
					</xsl:choose>

					<!--   Side End    -->

					<translevel>
						<xsl:value-of select="'B'"/>
					</translevel>

					<ClientRef>
						<xsl:value-of select="TradeRefID"/>
					</ClientRef>

					<Associated>
						<xsl:value-of select="TradeRefID"/>
					</Associated>

					<ExecAccount>
						<xsl:value-of select="'038354502'"/>
					</ExecAccount>

					<CustAccount>
						<xsl:value-of select="'038354502'"/>
					</CustAccount>

					<xsl:choose>
						<xsl:when test ="CounterParty='MS' or CounterParty='MSCO'">
							<ExecBkr>
								<xsl:value-of select="'MSCO'"/>
							</ExecBkr>
						</xsl:when>
						<xsl:otherwise>
							<ExecBkr>
								<xsl:value-of select="CounterParty"/>
							</ExecBkr>
						</xsl:otherwise>
					</xsl:choose>

					<SecType>
						<xsl:value-of select="'T'"/>
					</SecType>


					<xsl:variable name ="PRANA_SYMBOL" select ="Symbol"/>
					<xsl:variable name="PB_SYMBOL_NAME">
						<xsl:value-of select="document('../ReconMappingXml/SymbolMappingForStarPoint.xml')/SymbolMapping/PB[@Name='StarPoint']/SymbolData[@PranaSymbol=$PRANA_SYMBOL]/@PBSymbol"/>
					</xsl:variable>


					<xsl:choose>
						<xsl:when test ="$PB_SYMBOL_NAME !=''">
							<SecID>
								<xsl:value-of select="$PB_SYMBOL_NAME"/>
							</SecID>
						</xsl:when>
						<xsl:when test ="$PB_SYMBOL_NAME =''">
							<xsl:choose>
								<xsl:when test ="Asset = 'EquityOption'">
									<xsl:variable name ="varSymbolBef" select ="substring-before(Symbol,' ')"/>
									<xsl:variable name ="varSymbolAft" select ="substring-after(Symbol,' ')"/>
									<SecID>
										<xsl:value-of select="concat($varSymbolBef,'/',$varSymbolAft)"/>
									</SecID>
								</xsl:when>
								<xsl:otherwise>
									<SecID>
										<xsl:value-of select="Symbol"/>
									</SecID>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<SecID>
								<xsl:value-of select="Symbol"/>
							</SecID>
						</xsl:otherwise>
					</xsl:choose>

					<desc>
						<xsl:value-of select="FullSecurityName"/>
					</desc>

					<TDate>
						<xsl:value-of select="TradeDate"/>
					</TDate>

					<SDate>
						<xsl:value-of select="SettlementDate"/>
					</SDate>

					<CCY>
						<xsl:value-of select="CurrencySymbol"/>
					</CCY>

					<ExCode>
						<xsl:value-of select="''"/>
					</ExCode>

					<qty>
						<xsl:value-of select="AllocatedQty"/>
					</qty>

					<price>
						<xsl:value-of select="AveragePrice"/>
					</price>

					<type>
						<xsl:value-of select="'G'"/>
					</type>

					<prin>
						<xsl:value-of select="GrossAmount"/>
					</prin>

					<comm>
						<xsl:value-of select="CommissionCharged + TaxOnCommissions"/>
					</comm>

					<comtype>
						<xsl:value-of select="'F'"/>
					</comtype>

					<Othercharges>
						<xsl:value-of select="0"/>
					</Othercharges>

					<Taxfees>
						<xsl:value-of select="StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee"/>
					</Taxfees>

					<feesind>
						<xsl:value-of select="'F'"/>
					</feesind>

					<interest>
						<xsl:value-of select="0"/>
					</interest>

					<interestindicator>
						<xsl:value-of select="''"/>
					</interestindicator>

					<netamount>
						<xsl:value-of select="NetAmount"/>
					</netamount>

					<hsyind>
						<xsl:value-of select="''"/>
					</hsyind>

					<custbkr>
						<xsl:value-of select="''"/>
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
						<xsl:value-of select="' '"/>
					</acqdate>

					<instx>
						<xsl:value-of select="''"/>
					</instx>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>
				</ThirdPartyFlatFileDetail>

				<ThirdPartyFlatFileDetail>
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<transType>
						<xsl:value-of select="'TR001'"/>
					</transType>

					<xsl:choose>
						<xsl:when test ="TaxLotState = 'Allocated'">
							<TransStatus>
								<xsl:value-of select="'NEW'"/>
							</TransStatus>
						</xsl:when>
						<xsl:when test ="TaxLotState = 'Amemded'">
							<TransStatus>
								<xsl:value-of select="'COR'"/>
							</TransStatus>
						</xsl:when>
						<xsl:when test ="TaxLotState = 'Deleted'">
							<TransStatus>
								<xsl:value-of select="'CAN'"/>
							</TransStatus>
						</xsl:when>
						<xsl:otherwise>
							<TransStatus>
								<xsl:value-of select="'NEW'"/>
							</TransStatus>
						</xsl:otherwise>
					</xsl:choose>

					<BuySell>
						<xsl:value-of select="''"/>
					</BuySell>

					<LongShot>
						<xsl:value-of select="''"/>
					</LongShot>

					<!--   Side     -->

					<xsl:choose>
						<xsl:when test="Side='Buy' or Side='Buy to Open'">
							<PosType>
								<xsl:value-of select="'BL'"/>
							</PosType>
						</xsl:when>
						<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
							<PosType>
								<xsl:value-of select="'BC'"/>
							</PosType>
						</xsl:when>
						<xsl:when test="Side='Sell' or Side='Sell to Close'">
							<PosType>
								<xsl:value-of select="'SL'"/>
							</PosType>
						</xsl:when>
						<xsl:when test="Side='Sell short' or Side='Sell to Open'">
							<PosType>
								<xsl:value-of select="'SS'"/>
							</PosType>
						</xsl:when>
						<xsl:otherwise>
							<PosType>
								<xsl:value-of select="''"/>
							</PosType>
						</xsl:otherwise>
					</xsl:choose>

					<!--   Side End    -->

					<translevel>
						<xsl:value-of select="'A'"/>
					</translevel>

					<ClientRef>
						<xsl:value-of select="concat(TradeRefID,'A')"/>
					</ClientRef>

					<Associated>
						<xsl:value-of select="TradeRefID"/>
					</Associated>

					<ExecAccount>
						<xsl:value-of select="'038354502'"/>
					</ExecAccount>

					<CustAccount>
						<xsl:value-of select="FundMappedName"/>
					</CustAccount>

					<xsl:choose>
						<xsl:when test ="CounterParty='MS' or CounterParty='MSCO'">
							<ExecBkr>
								<xsl:value-of select="'MSCO'"/>
							</ExecBkr>
						</xsl:when>
						<xsl:otherwise>
							<ExecBkr>
								<xsl:value-of select="CounterParty"/>
							</ExecBkr>
						</xsl:otherwise>
					</xsl:choose>


					<SecType>
						<xsl:value-of select="'T'"/>
					</SecType>

					<xsl:variable name ="PRANA_SYMBOL" select ="Symbol"/>
					<xsl:variable name="PB_SYMBOL_NAME">
						<xsl:value-of select="document('../ReconMappingXml/SymbolMappingForStarPoint.xml')/SymbolMapping/PB[@Name='StarPoint']/SymbolData[@PranaSymbol=$PRANA_SYMBOL]/@PBSymbol"/>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test ="$PB_SYMBOL_NAME !=''">
							<SecID>
								<xsl:value-of select="$PB_SYMBOL_NAME"/>
							</SecID>
						</xsl:when>
						<xsl:when test ="$PB_SYMBOL_NAME =''">
							<xsl:choose>
								<xsl:when test ="Asset = 'EquityOption'">
									<xsl:variable name ="varSymbolBef" select ="substring-before(Symbol,' ')"/>
									<xsl:variable name ="varSymbolAft" select ="substring-after(Symbol,' ')"/>
									<SecID>
										<xsl:value-of select="concat($varSymbolBef,'/',$varSymbolAft)"/>
									</SecID>
								</xsl:when>
								<xsl:otherwise>
									<SecID>
										<xsl:value-of select="Symbol"/>
									</SecID>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<SecID>
								<xsl:value-of select="Symbol"/>
							</SecID>
						</xsl:otherwise>
					</xsl:choose>

					<desc>
						<xsl:value-of select="FullSecurityName"/>
					</desc>

					<TDate>
						<xsl:value-of select="TradeDate"/>
					</TDate>

					<SDate>
						<xsl:value-of select="SettlementDate"/>
					</SDate>

					<CCY>
						<xsl:value-of select="CurrencySymbol"/>
					</CCY>

					<ExCode>
						<xsl:value-of select="''"/>
					</ExCode>

					<qty>
						<xsl:value-of select="AllocatedQty"/>
					</qty>

					<price>
						<xsl:value-of select="AveragePrice"/>
					</price>

					<type>
						<xsl:value-of select="'G'"/>
					</type>

					<prin>
						<xsl:value-of select="GrossAmount"/>
					</prin>

					<comm>
						<xsl:value-of select="CommissionCharged + TaxOnCommissions"/>
					</comm>

					<comtype>
						<xsl:value-of select="'F'"/>
					</comtype>

					<Othercharges>
						<xsl:value-of select="0"/>
					</Othercharges>

					<Taxfees>
						<xsl:value-of select="StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee"/>
					</Taxfees>

					<feesind>
						<xsl:value-of select="'F'"/>
					</feesind>

					<interest>
						<xsl:value-of select="0"/>
					</interest>

					<interestindicator>
						<xsl:value-of select="''"/>
					</interestindicator>

					<netamount>
						<xsl:value-of select="NetAmount"/>
					</netamount>

					<hsyind>
						<xsl:value-of select="''"/>
					</hsyind>

					<custbkr>
						<xsl:value-of select="''"/>
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
						<xsl:value-of select="' '"/>
					</acqdate>

					<instx>
						<xsl:value-of select="''"/>
					</instx>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>
				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
