<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="MonthName">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month=01">
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month=02">
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month=03">
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month=04">
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month=05">
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month=06">
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month=07">
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month=08">
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month=09">
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month=10">
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month=11">
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month=12">
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


	<xsl:template name="Count">
		<xsl:param name="Symbol"/>
		<xsl:value-of select="count(//ThirdPartyFlatFileDetail[Symbol=$Symbol])"/>
	</xsl:template>

	<xsl:template name="SumPrice">
		<xsl:param name="Symbol"/>
		<xsl:value-of select="sum(//ThirdPartyFlatFileDetail[Symbol=$Symbol]/AveragePrice)"/>
	</xsl:template>

	<xsl:template name="BLK">
		<xsl:param name="ID"/>
		<xsl:param name="Symbol"/>
		<xsl:choose>
			<xsl:when test="$ID = (//ThirdPartyFlatFileDetail[Symbol=$Symbol]/PBUniqueID)">
				<xsl:value-of select="(//ThirdPartyFlatFileDetail[Symbol=$Symbol]/PBUniqueID)"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="DateFormat">
		<xsl:param name="Date"/>
		<xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
	</xsl:template>

	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>
				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>

				<FileHeader>
					<xsl:value-of select="'true'"/>
				</FileHeader>

				<FileFooter>
					<xsl:value-of select="'true'"/>
				</FileFooter>

				<TaxLotState>
					<xsl:value-of select="''"/>
				</TaxLotState>

				<LOCALREF>
					<xsl:value-of select="'LOCALREF'"/>
				</LOCALREF>

				<CFID>
					<xsl:value-of select="'CFID'"/>
				</CFID>

				<ROUTECD>
					<xsl:value-of select="'ROUTECD'"/>
				</ROUTECD>

				<TIRORDERID>
					<xsl:value-of select="'TIRORDERID'"/>
				</TIRORDERID>

				<TIRPIECE>
					<xsl:value-of select="'TIRPIECE'"/>
				</TIRPIECE>

				<TIRSEQ>
					<xsl:value-of select="'TIRSEQ'"/>
				</TIRSEQ>

				<SECIDTYPE>
					<xsl:value-of select="'SECIDTYPE'"/>
				</SECIDTYPE>

				<SECURITYID>
					<xsl:value-of select="'SECURITYID'"/>
				</SECURITYID>

				<DESCRIPTION1>
					<xsl:value-of select="'DESCRIPTION1'"/>
				</DESCRIPTION1>

				<DESCRIPTION2>
					<xsl:value-of select="'DESCRIPTION2'"/>
				</DESCRIPTION2>

				<DESCRIPTION3>
					<xsl:value-of select="'DESCRIPTION3'"/>
				</DESCRIPTION3>

				<DESCRIPTION4>
					<xsl:value-of select="'DESCRIPTION4'"/>
				</DESCRIPTION4>

				<TRADEDATE>
					<xsl:value-of select="'TRADEDATE'"/>
				</TRADEDATE>

				<SETLDATE>
					<xsl:value-of select="'SETLDATE'"/>
				</SETLDATE>

				<QUANTITY>
					<xsl:value-of select="'QUANTITY'"/>
				</QUANTITY>

				<QUANTITYDESC>
					<xsl:value-of select="'QUANTITYDESC'"/>
				</QUANTITYDESC>

				<NETMONEY>
					<xsl:value-of select="'NETMONEY'"/>
				</NETMONEY>

				<CASHACCOUNT>
					<xsl:value-of select="'CASHACCOUNT'"/>
				</CASHACCOUNT>

				<SECACCOUNT>
					<xsl:value-of select="'SECACCOUNT'"/>
				</SECACCOUNT>

				<TRADECURRID>
					<xsl:value-of select="'TRADECURRID'"/>
				</TRADECURRID>

				<SETLCURRID>
					<xsl:value-of select="'SETLCURRID'"/>
				</SETLCURRID>

				<BSIND>
					<xsl:value-of select="'BSIND'"/>
				</BSIND>


				<INSTTYP>
					<xsl:value-of select="'INSTTYP'"/>
				</INSTTYP>

				<PRICE>
					<xsl:value-of select="'PRICE'"/>
				</PRICE>

				<COMMISSION>
					<xsl:value-of select="'COMMISSION'"/>
				</COMMISSION>

				<STAMPTAX>
					<xsl:value-of select="'STAMPTAX'"/>
				</STAMPTAX>

				<LOCALCHGS>
					<xsl:value-of select="'LOCALCHGS'"/>
				</LOCALCHGS>

				<INTEREST>
					<xsl:value-of select="'INTEREST'"/>
				</INTEREST>

				<PRINCIPAL>
					<xsl:value-of select="'PRINCIPAL'"/>
				</PRINCIPAL>

				<SECFEE>
					<xsl:value-of select="'SECFEE'"/>
				</SECFEE>

				<EXECBROKER>
					<xsl:value-of select="'EXECBROKER'"/>
				</EXECBROKER>


				<BROKEROS>
					<xsl:value-of select="'BROKEROS'"/>
				</BROKEROS>

				<TRAILERCD1>
					<xsl:value-of select="'TRAILERCD1'"/>
				</TRAILERCD1>

				<TRAILERCD2>
					<xsl:value-of select="'TRAILERCD2'"/>
				</TRAILERCD2>

				<TRAILERCD3>
					<xsl:value-of select="'TRAILERCD3'"/>
				</TRAILERCD3>

				<BLOTTERCD>
					<xsl:value-of select="'BLOTTERCD'"/>
				</BLOTTERCD>

				<CLRNGHSE>
					<xsl:value-of select="'CLRNGHSE'"/>
				</CLRNGHSE>


				<CLRAGNTCD>
					<xsl:value-of select="'CLRAGNTCD'"/>
				</CLRAGNTCD>

				<CLRAGNT1>
					<xsl:value-of select="'CLRAGNT1'"/>
				</CLRAGNT1>

				<CLRAGNT2>
					<xsl:value-of select="'CLRAGNT2'"/>
				</CLRAGNT2>

				<CLRAGNT3>
					<xsl:value-of select="'CLRAGNT3'"/>
				</CLRAGNT3>

				<CLRAGNT4>
					<xsl:value-of select="'CLRAGNT4'"/>
				</CLRAGNT4>

				<CNTRPRTYCD>
					<xsl:value-of select="'CNTRPRTYCD'"/>
				</CNTRPRTYCD>


				<CNTRPTY1>
					<xsl:value-of select="'CNTRPTY1'"/>
				</CNTRPTY1>

				<CNTRPTY2>
					<xsl:value-of select="'CNTRPTY2'"/>
				</CNTRPTY2>

				<CNTRPTY3>
					<xsl:value-of select="'CNTRPTY3'"/>
				</CNTRPTY3>

				<CNTRPTY4>
					<xsl:value-of select="'CNTRPTY4'"/>
				</CNTRPTY4>

				<INSTRUCT>
					<xsl:value-of select="'INSTRUCT'"/>
				</INSTRUCT>

				<CEDELAKV>
					<xsl:value-of select="'CEDELAKV'"/>
				</CEDELAKV>


				<ORIGLOCALREF>
					<xsl:value-of select="'ORIGLOCALREF'"/>
				</ORIGLOCALREF>

				<NOTES>
					<xsl:value-of select="'NOTES'"/>
				</NOTES>

				<FILLER>
					<xsl:value-of select="'FILLER'"/>
				</FILLER>

				<FILLER1>
					<xsl:value-of select="'FILLER1'"/>
				</FILLER1>

				<RR>
					<xsl:value-of select="'RR'"/>
				</RR>

				<SETLCOUNTRYCD>
					<xsl:value-of select="'SETLCOUNTRYCD'"/>
				</SETLCOUNTRYCD>

				<INSTRUMENTTYPE>
					<xsl:value-of select="'INSTRUMENTTYPE'"/>
				</INSTRUMENTTYPE>


				<COMMISSIONRATE>
					<xsl:value-of select="'COMMISSIONRATE'"/>
				</COMMISSIONRATE>

				<COMPANYNO>
					<xsl:value-of select="'COMPANYNO'"/>
				</COMPANYNO>

				<Filler2>
					<xsl:value-of select="'Filler2'"/>
				</Filler2>

				<Filler3>
					<xsl:value-of select="'Filler3'"/>
				</Filler3>

				<Filler4>
					<xsl:value-of select="'Filler4'"/>
				</Filler4>

				<Filler5>
					<xsl:value-of select="'Filler5'"/>
				</Filler5>

				<Filler6>
					<xsl:value-of select="'Filler6'"/>
				</Filler6>


				<GPF2IDCode>
					<xsl:value-of select="'GPF2 ID Code'"/>
				</GPF2IDCode>


				<GPF2Amount>
					<xsl:value-of select="'GPF2 Amount'"/>
				</GPF2Amount>

				<GPF2CurrencyCode>
					<xsl:value-of select="'GPF2 Currency Code'"/>
				</GPF2CurrencyCode>

				<GPF2AddSubtract>
					<xsl:value-of select="'GPF2 Add Subtract'"/>
				</GPF2AddSubtract>



				<GPF3IDCode>
					<xsl:value-of select="'GPF3 ID Code'"/>
				</GPF3IDCode>

				<GPF3Amount>
					<xsl:value-of select="'GPF3 Amount'"/>
				</GPF3Amount>

				<GPF3CurrencyCode>
					<xsl:value-of select="'GPF3 Currency Code'"/>
				</GPF3CurrencyCode>

				<GPF3AddSubtract>
					<xsl:value-of select="'GPF3 Add Subtract'"/>
				</GPF3AddSubtract>

				<GPF4IDCode>
					<xsl:value-of select="'GPF4 ID Code'"/>
				</GPF4IDCode>

				<GPF4Amount>
					<xsl:value-of select="'GPF4 Amount'"/>
				</GPF4Amount>

				<GPF4CurrencyCode>
					<xsl:value-of select="'GPF4 Currency Code'"/>
				</GPF4CurrencyCode>

				<GPF4AddSubtract>
					<xsl:value-of select="'GPF4 Add Subtract'"/>
				</GPF4AddSubtract>

				<GPF5IDCode>
					<xsl:value-of select="'GPF5 ID Code'"/>
				</GPF5IDCode>

				<GPF5Amount>
					<xsl:value-of select="'GPF5 Amount'"/>
				</GPF5Amount>

				<GPF5CurrencyCode>
					<xsl:value-of select="'GPF5 Currency Code'"/>
				</GPF5CurrencyCode>

				<GPF5AddSubtract>
					<xsl:value-of select="'GPF5 Add Subtract'"/>
				</GPF5AddSubtract>

				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<xsl:variable name="varNetamount">
					<xsl:choose>
						<xsl:when test="contains(Side,'Buy')">
							<xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) + CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
						</xsl:when>
						<xsl:when test="contains(Side,'Sell')">
							<xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) - (CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
						</xsl:when>
					</xsl:choose>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test ="TaxLotState!='Amemded'">
						<ThirdPartyFlatFileDetail>

							<xsl:variable name="BLK">
								<xsl:call-template name="BLK">
									<xsl:with-param name="ID" select="PBUniqueID"/>
									<xsl:with-param name="Symbol" select="Symbol"/>
								</xsl:call-template>
							</xsl:variable>

							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>

							<FileHeader>
								<xsl:value-of select="'true'"/>
							</FileHeader>

							<FileFooter>
								<xsl:value-of select="'true'"/>
							</FileFooter>


							<TaxLotState>
								<xsl:value-of select="TaxLotState"/>
							</TaxLotState>



							<LOCALREF>
								<xsl:value-of select="concat('PFJ',substring(EntityID,string-length(EntityID)-6,string-length(EntityID)))"/>
							</LOCALREF>

							<CFID>
								<xsl:value-of select="concat('PFJ',substring(EntityID,string-length(EntityID)-6,string-length(EntityID)))"/>
							</CFID>

							<ROUTECD>
								<xsl:value-of select="'PSHG'"/>
							</ROUTECD>

							<TIRORDERID>
								<xsl:value-of select="concat('BLK', $BLK)"/>
							</TIRORDERID>

							<TIRPIECE>
								<xsl:value-of select="''"/>
							</TIRPIECE>

							<TIRSEQ>
								<xsl:value-of select="''"/>
							</TIRSEQ>

							<SECIDTYPE>

								<xsl:choose>
									<xsl:when test="CurrencySymbol!='USD'">
										<xsl:choose>
											<xsl:when test="ISIN!=''">
												<xsl:value-of select="'N'"/>
											</xsl:when>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="CUSIP!=''">
												<xsl:value-of select="'C'"/>
											</xsl:when>
											<xsl:when test="SEDOL!=''">
												<xsl:value-of select="'D'"/>
											</xsl:when>

											<xsl:when test="contains(Asset,'Option')">
												<xsl:value-of select="'O'"/>
											</xsl:when>

											<xsl:when test="Asset='Equity'">
												<xsl:value-of select="'S'"/>
											</xsl:when>

										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>


							</SECIDTYPE>

							<SECURITYID>
								<xsl:choose>
									<xsl:when test="CurrencySymbol!='USD'">
										<xsl:choose>
											<xsl:when test="ISIN!=''">
												<xsl:value-of select="ISIN"/>
											</xsl:when>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="CUSIP!=''">
												<xsl:value-of select="CUSIP"/>
											</xsl:when>

											<xsl:when test="SEDOL!=''">
												<xsl:value-of select="SEDOL"/>
											</xsl:when>

											<xsl:when test="contains(Asset,'Option')">
												<xsl:value-of select="OSIOptionSymbol"/>
											</xsl:when>

											<xsl:when test="Asset='Equity'">
												<xsl:value-of select="Symbol"/>
											</xsl:when>
											<xsl:when test="Asset='FixedIncome'">
												<xsl:value-of select="CUSIP"/>
											</xsl:when>

										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>

							</SECURITYID>


							<xsl:variable name="varCounterParty">
								<xsl:value-of select="CounterParty"/>
							</xsl:variable>
							<DESCRIPTION1>
								<xsl:value-of select="concat('PB ',$varCounterParty)"/>
							</DESCRIPTION1>

							<DESCRIPTION2>
								<xsl:value-of select="''"/>
							</DESCRIPTION2>

							<DESCRIPTION3>
								<xsl:choose>
									<xsl:when test="Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'S*PERS'"/>
									</xsl:when>



									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>

								</xsl:choose>
							</DESCRIPTION3>

							<DESCRIPTION4>
								<xsl:value-of select="''"/>
							</DESCRIPTION4>

							<xsl:variable name="TradeDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="TradeDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="SettlementDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="SettlementDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="varMonthCode">
								<xsl:call-template name="MonthName">
									<xsl:with-param name="Month" select="substring-before($TradeDate,'/')"/>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="varSMonthCode">
								<xsl:call-template name="MonthName">
									<xsl:with-param name="Month" select="substring-before($SettlementDate,'/')"/>
								</xsl:call-template>
							</xsl:variable>

							<TRADEDATE>
								<xsl:value-of select="concat(substring-before(substring-after($TradeDate,'/'),'/'),'-',$varMonthCode,'-',substring(substring-after(substring-after($TradeDate,'/'),'/'),3,2))"/>
							</TRADEDATE>


							<SETLDATE>
								<xsl:value-of select="concat(substring-before(substring-after($SettlementDate,'/'),'/'),'-',$varSMonthCode,'-',substring(substring-after(substring-after($SettlementDate,'/'),'/'),3,2))"/>
							</SETLDATE>


							<QUANTITY>
								<xsl:value-of select="OrderQty"/>
							</QUANTITY>

							<QUANTITYDESC>
								<xsl:value-of select="''"/>
							</QUANTITYDESC>

							<NETMONEY>
								<xsl:value-of select="''"/>
							</NETMONEY>

							<xsl:variable name = "PRANA_FUND_NAME">
								<xsl:value-of select="''"/>
							</xsl:variable>

							<xsl:variable name ="THIRDPARTY_FUND_CODE">
								<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='']/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
							</xsl:variable>


							<xsl:variable name="varAccountName">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
										<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_FUND_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<CASHACCOUNT>
								<xsl:choose>
									<xsl:when test="contains(FundAccntNo,'PFJ004002')">
										<xsl:value-of select="'3DT8138710'"/>
									</xsl:when>
									<xsl:when test="contains(FundAccntNo,'PFJ001008')">
										<xsl:value-of select="'3DT3180460'"/>
									</xsl:when>
									<xsl:when test="contains(FundAccntNo,'PFJ002006')">
										<xsl:value-of select="'3DT3180460'"/>
									</xsl:when>
								</xsl:choose>
							</CASHACCOUNT>

							<SECACCOUNT>
								<xsl:choose>
									<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Sell' or Side='Sell to Close'">
										<xsl:value-of select="concat(FundAccntNo,'2')"/>
									</xsl:when>

									<xsl:when test="Side='Buy to Close' or Side='Buy to Cover' or Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="concat(FundAccntNo,'3')"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="FundAccntNo"/>
									</xsl:otherwise>

								</xsl:choose>
							</SECACCOUNT>

							<TRADECURRID>
								<xsl:value-of select="SettlCurrency"/>
							</TRADECURRID>

							<SETLCURRID>
								<xsl:value-of select="SettlCurrency"/>
							</SETLCURRID>

							<BSIND>
								<xsl:choose>
									<xsl:when test="contains(Side,'Buy')">
										<xsl:value-of select="'B'"/>
									</xsl:when>
									<xsl:when test="contains(Side,'Sell')">
										<xsl:value-of select="'S'"/>
									</xsl:when>
								</xsl:choose>
							</BSIND>


							<INSTTYP>
								<xsl:choose>
									<xsl:when test="TaxLotState='Allocated'">
										<xsl:value-of select ="'N'"/>
									</xsl:when>

									<xsl:when test="TaxLotState='Deleted'">
										<xsl:value-of select ="'Y'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="'SENT'"/>
									</xsl:otherwise>
								</xsl:choose>
							</INSTTYP>

							<xsl:variable name="AvgPrice1">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="AvgPrice"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="AvgPrice * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="AvgPrice div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<PRICE>
								<xsl:value-of select="format-number($AvgPrice1,'0.#######')"/>
							</PRICE>

							<xsl:variable name="varCommission">
								<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
							</xsl:variable>

							<xsl:variable name="varCommission1">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="$varCommission"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="$varCommission * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="$varCommission div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<COMMISSION>
								<xsl:value-of select="format-number($varCommission1,'0.##')"/>
							</COMMISSION>

							<STAMPTAX>
								<xsl:value-of select="''"/>
							</STAMPTAX>

							<xsl:variable name="varOtherFees">
								<xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
							</xsl:variable>

							<xsl:variable name="varOtherFees1">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="$varOtherFees"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="$varOtherFees * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="$varOtherFees div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<LOCALCHGS>
								<xsl:value-of select="''"/>
							</LOCALCHGS>

							<INTEREST>
								<xsl:value-of select="''"/>
							</INTEREST>

							<PRINCIPAL>
								<xsl:value-of select="''"/>
							</PRINCIPAL>

							<SECFEE>
								<xsl:value-of select="''"/>
							</SECFEE>

							<xsl:variable name="PB_NAME" select="''"/>
							<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
							<xsl:variable name="THIRDPARTY_BROKER">
								<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
							</xsl:variable>
							<EXECBROKER>
								<xsl:value-of select="''"/>
							</EXECBROKER>


							<BROKEROS>
								<xsl:value-of select="''"/>
							</BROKEROS>

							<TRAILERCD1>
								<xsl:choose>

									<xsl:when test="Asset='EquityOption'">
										<xsl:choose>
											<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Sell short' or Side='Sell to Open'">
												<xsl:value-of select="'O'"/>
											</xsl:when>
											<xsl:when test="Side='Sell' or Side='Sell to Close' or Side='Buy to Close'">
												<xsl:value-of select="'C'"/>
											</xsl:when>
										</xsl:choose>

									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>

							</TRAILERCD1>

							<TRAILERCD2>
								<xsl:value-of select="''"/>
							</TRAILERCD2>

							<TRAILERCD3>
								<xsl:choose>
									<xsl:when test=" Side='Sell short'">
										<xsl:value-of select="'S'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' or Side='Sell to Close' or Side='Buy to Close'">
										<xsl:value-of select="''"/>
									</xsl:when>
								</xsl:choose>
							</TRAILERCD3>

							<BLOTTERCD>
								<xsl:value-of select="'49'"/>
							</BLOTTERCD>

							<CLRNGHSE>
								<xsl:value-of select="''"/>
							</CLRNGHSE>


							<CLRAGNTCD>
								<xsl:value-of select="$varCounterParty"/>
							</CLRAGNTCD>

							<CLRAGNT1>
								<xsl:value-of select="''"/>
							</CLRAGNT1>

							<CLRAGNT2>
								<xsl:value-of select="''"/>
							</CLRAGNT2>

							<CLRAGNT3>
								<xsl:value-of select="''"/>
							</CLRAGNT3>

							<CLRAGNT4>
								<xsl:value-of select="''"/>
							</CLRAGNT4>

							<CNTRPRTYCD>
								<xsl:value-of select="''"/>
							</CNTRPRTYCD>


							<CNTRPTY1>
								<xsl:value-of select="''"/>
							</CNTRPTY1>

							<CNTRPTY2>
								<xsl:value-of select="''"/>
							</CNTRPTY2>

							<CNTRPTY3>
								<xsl:value-of select="''"/>
							</CNTRPTY3>

							<CNTRPTY4>
								<xsl:value-of select="''"/>
							</CNTRPTY4>

							<INSTRUCT>
								<xsl:value-of select="''"/>
							</INSTRUCT>

							<CEDELAKV>
								<xsl:value-of select="''"/>
							</CEDELAKV>


							<ORIGLOCALREF>
								<xsl:value-of select="concat('PFJ',substring(EntityID,string-length(EntityID)-6,string-length(EntityID)))"/>
							</ORIGLOCALREF>

							<NOTES>
								<xsl:value-of select="''"/>
							</NOTES>

							<FILLER>
								<xsl:value-of select="''"/>
							</FILLER>

							<FILLER1>
								<xsl:value-of select="''"/>
							</FILLER1>

							<RR>
								<xsl:value-of select="''"/>
							</RR>

							<SETLCOUNTRYCD>


								<xsl:choose>
									<xsl:when test="contains(Asset,'EquityOption')">
										<xsl:value-of select="'US'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='USD'">
										<xsl:value-of select="'US'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='AED'">
										<xsl:value-of select="'AE'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='ARS'">
										<xsl:value-of select="'AR'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='BRL'">
										<xsl:value-of select="'BR'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='CAD'">
										<xsl:value-of select="'CA'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='CHF'">
										<xsl:value-of select="'CH'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='COP'">
										<xsl:value-of select="'CO'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='EUR'">
										<xsl:value-of select="'EU'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='GBP'">
										<xsl:value-of select="'GB'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='IDR'">
										<xsl:value-of select="'ID'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='INR'">
										<xsl:value-of select="'IN'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='JPY'">
										<xsl:value-of select="'JP'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='KES'">
										<xsl:value-of select="'KE'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='KRW'">
										<xsl:value-of select="'KR'"/>
									</xsl:when>

									<xsl:when test="CurrencySymbol='LKR'">
										<xsl:value-of select="'LK'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='MYR'">
										<xsl:value-of select="'MY'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='PEN'">
										<xsl:value-of select="'PE'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='PHP'">
										<xsl:value-of select="'PH'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='PLN'">
										<xsl:value-of select="'PL'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='RUB'">
										<xsl:value-of select="'RU'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='TZS'">
										<xsl:value-of select="'TZ'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='VND'">
										<xsl:value-of select="'VN'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='ZAR'">
										<xsl:value-of select="'ZA'"/>
									</xsl:when>

									<xsl:when test="CurrencySymbol='AUD'">
										<xsl:value-of select="'AU'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='BDT'">
										<xsl:value-of select="'BT'"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SETLCOUNTRYCD>

							<INSTRUMENTTYPE>
								<xsl:value-of select="''"/>
							</INSTRUMENTTYPE>


							<COMMISSIONRATE>
								<xsl:value-of select="''"/>
							</COMMISSIONRATE>

							<COMPANYNO>
								<xsl:value-of select="''"/>
							</COMPANYNO>

							<Filler2>
								<xsl:value-of select="''"/>
							</Filler2>

							<Filler3>
								<xsl:value-of select="''"/>
							</Filler3>

							<Filler4>
								<xsl:value-of select="''"/>
							</Filler4>

							<Filler5>
								<xsl:value-of select="''"/>
							</Filler5>

							<Filler6>
								<xsl:value-of select="''"/>
							</Filler6>


							<GPF2IDCode>
								<xsl:value-of select="''"/>
							</GPF2IDCode>


							<GPF2Amount>
								<xsl:value-of select="''"/>
							</GPF2Amount>

							<GPF2CurrencyCode>
								<xsl:value-of select="''"/>
							</GPF2CurrencyCode>

							<GPF2AddSubtract>
								<xsl:value-of select="''"/>
							</GPF2AddSubtract>



							<GPF3IDCode>
								<xsl:value-of select="''"/>
							</GPF3IDCode>

							<GPF3Amount>
								<xsl:value-of select="''"/>
							</GPF3Amount>

							<GPF3CurrencyCode>
								<xsl:value-of select="''"/>
							</GPF3CurrencyCode>

							<GPF3AddSubtract>
								<xsl:value-of select="''"/>
							</GPF3AddSubtract>

							<GPF4IDCode>
								<xsl:value-of select="''"/>
							</GPF4IDCode>

							<GPF4Amount>
								<xsl:value-of select="''"/>
							</GPF4Amount>

							<GPF4CurrencyCode>
								<xsl:value-of select="''"/>
							</GPF4CurrencyCode>

							<GPF4AddSubtract>
								<xsl:value-of select="''"/>
							</GPF4AddSubtract>

							<GPF5IDCode>
								<xsl:value-of select="''"/>
							</GPF5IDCode>

							<GPF5Amount>
								<xsl:value-of select="''"/>
							</GPF5Amount>

							<GPF5CurrencyCode>
								<xsl:value-of select="''"/>
							</GPF5CurrencyCode>

							<GPF5AddSubtract>
								<xsl:value-of select="''"/>
							</GPF5AddSubtract>

							<EntityID>
								<xsl:value-of select="EntityID"/>
							</EntityID>

						</ThirdPartyFlatFileDetail>
					</xsl:when>

					<xsl:otherwise>
						<xsl:if test ="number(OldExecutedQuantity)">
							<ThirdPartyFlatFileDetail>


								<xsl:variable name="BLK">
									<xsl:call-template name="BLK">
										<xsl:with-param name="ID" select="PBUniqueID"/>
										<xsl:with-param name="Symbol" select="Symbol"/>
									</xsl:call-template>
								</xsl:variable>
								<RowHeader>
									<xsl:value-of select ="'false'"/>
								</RowHeader>

								<FileHeader>
									<xsl:value-of select="'true'"/>
								</FileHeader>

								<FileFooter>
									<xsl:value-of select="'true'"/>
								</FileFooter>


								<TaxLotState>
									<xsl:value-of select="'Deleted'"/>
								</TaxLotState>


								<LOCALREF>
									<xsl:value-of select="concat('PFJ',substring(EntityID,string-length(EntityID)-6,string-length(EntityID)))"/>
								</LOCALREF>

								<CFID>
									<xsl:value-of select="concat('PFJ',substring(EntityID,string-length(EntityID)-6,string-length(EntityID)))"/>
								</CFID>

								<ROUTECD>
									<xsl:value-of select="'PSHG'"/>
								</ROUTECD>

								<TIRORDERID>
									<xsl:value-of select="concat('BLK', $BLK)"/>
								</TIRORDERID>

								<TIRPIECE>
									<xsl:value-of select="''"/>
								</TIRPIECE>

								<TIRSEQ>
									<xsl:value-of select="''"/>
								</TIRSEQ>

								<SECIDTYPE>
									<xsl:choose>
										<xsl:when test="CurrencySymbol!='USD'">
											<xsl:choose>
												<xsl:when test="ISIN!=''">
													<xsl:value-of select="'N'"/>
												</xsl:when>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="CUSIP!=''">
													<xsl:value-of select="'C'"/>
												</xsl:when>
												<xsl:when test="SEDOL!=''">
													<xsl:value-of select="'D'"/>
												</xsl:when>
												<xsl:when test="Asset='Equity'">
													<xsl:value-of select="'S'"/>
												</xsl:when>
												<xsl:when test="contains(Asset,'Option')">
													<xsl:value-of select="'O'"/>
												</xsl:when>


											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>

								</SECIDTYPE>

								<SECURITYID>
									<xsl:choose>
										<xsl:when test="CurrencySymbol!='USD'">
											<xsl:choose>
												<xsl:when test="ISIN!=''">
													<xsl:value-of select="ISIN"/>
												</xsl:when>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="CUSIP!=''">
													<xsl:value-of select="'CUSIP'"/>
												</xsl:when>
												<xsl:when test="SEDOL!=''">
													<xsl:value-of select="SEDOL"/>
												</xsl:when>
												<xsl:when test="contains(Asset,'Option')">
													<xsl:value-of select="OSIOptionSymbol"/>
												</xsl:when>

												<xsl:when test="Asset='Equity'">
													<xsl:value-of select="Symbol"/>
												</xsl:when>
												<xsl:when test="Asset='FixedIncome'">
													<xsl:value-of select="CUSIP"/>
												</xsl:when>

											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>

								</SECURITYID>


								<xsl:variable name="varCounterParty">
									<xsl:value-of select="CounterParty"/>
								</xsl:variable>
								<DESCRIPTION1>
									<xsl:value-of select="concat('PB ',$varCounterParty)"/>
								</DESCRIPTION1>

								<DESCRIPTION2>
									<xsl:value-of select="''"/>
								</DESCRIPTION2>

								<DESCRIPTION3>
									<xsl:choose>
										<xsl:when test="OldSide='Sell short' or OldSide='Sell to Open'">
											<xsl:value-of select="'S*PERS'"/>
										</xsl:when>



										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>

									</xsl:choose>
								</DESCRIPTION3>

								<DESCRIPTION4>
									<xsl:value-of select="''"/>
								</DESCRIPTION4>

								<xsl:variable name="varOldTradeDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="OldTradeDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>
								<xsl:variable name="varOldSettleDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="OldSettlementDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>

								<xsl:variable name="varMonthCode">
									<xsl:call-template name="MonthName">
										<xsl:with-param name="Month" select="substring-before($varOldTradeDate,'/')"/>
									</xsl:call-template>
								</xsl:variable>

								<xsl:variable name="varSMonthCode">
									<xsl:call-template name="MonthName">
										<xsl:with-param name="Month" select="substring-before($varOldSettleDate,'/')"/>
									</xsl:call-template>
								</xsl:variable>



								<TRADEDATE>
									<xsl:value-of select="concat(substring-before(substring-after($varOldTradeDate,'/'),'/'),'-',$varMonthCode,'-',substring(substring-after(substring-after($varOldTradeDate,'/'),'/'),3,2))"/>
								</TRADEDATE>

								<SETLDATE>
									<xsl:value-of select="concat(substring-before(substring-after($varOldSettleDate,'/'),'/'),'-',$varSMonthCode,'-', substring(substring-after(substring-after($varOldSettleDate,'/'),'/'),3,2))"/>
								</SETLDATE>



								<QUANTITY>
									<xsl:value-of select="OldExecutedQuantity"/>
								</QUANTITY>

								<QUANTITYDESC>
									<xsl:value-of select="''"/>
								</QUANTITYDESC>

								<NETMONEY>
									<xsl:value-of select="''"/>
								</NETMONEY>

								<xsl:variable name = "PRANA_FUND_NAME">
									<xsl:value-of select="''"/>
								</xsl:variable>

								<xsl:variable name ="THIRDPARTY_FUND_CODE">
									<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='']/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
								</xsl:variable>


								<xsl:variable name="varAccountName">
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
											<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$PRANA_FUND_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<CASHACCOUNT>
									<xsl:choose>
										<xsl:when test="contains(FundAccntNo,'PFJ004002')">
											<xsl:value-of select="'3DT8138710'"/>
										</xsl:when>
										<xsl:when test="contains(FundAccntNo,'PFJ001008')">
											<xsl:value-of select="'3DT3180460'"/>
										</xsl:when>
										<xsl:when test="contains(FundAccntNo,'PFJ002006')">
											<xsl:value-of select="'3DT3180460'"/>
										</xsl:when>
									</xsl:choose>
								</CASHACCOUNT>

								<SECACCOUNT>
									<xsl:choose>
										<xsl:when test="OldSide='Buy' or OldSide='Buy to Open' or OldSide='Sell' or OldSide='Sell to Close'">
											<xsl:value-of select="concat(FundAccntNo,'2')"/>
										</xsl:when>

										<xsl:when test="OldSide='Buy to Close' or OldSide='Buy to Cover' or OldSide='Sell short' or OldSide='Sell to Open'">
											<xsl:value-of select="concat(FundAccntNo,'3')"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="FundAccntNo"/>
										</xsl:otherwise>

									</xsl:choose>
								</SECACCOUNT>

								<TRADECURRID>
									<xsl:value-of select="OldSettlCurrency"/>
								</TRADECURRID>

								<SETLCURRID>
									<xsl:value-of select="OldSettlCurrency"/>
								</SETLCURRID>

								<BSIND>
									<xsl:choose>
										<xsl:when test="contains(OldSide,'Buy')">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="contains(OldSide,'Sell')">
											<xsl:value-of select="'S'"/>
										</xsl:when>
									</xsl:choose>
								</BSIND>


								<INSTTYP>
									<xsl:value-of select ="'Y'"/>
								</INSTTYP>


								<xsl:variable name="AvgPrice1">
									<xsl:choose>
										<xsl:when test="SettlCurrFxRate=0">
											<xsl:value-of select="OldAvgPrice"/>
										</xsl:when>
										<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
											<xsl:value-of select="OldAvgPrice * SettlCurrFxRate"/>
										</xsl:when>

										<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
											<xsl:value-of select="OldAvgPrice div SettlCurrFxRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<PRICE>
									<xsl:value-of select="format-number($AvgPrice1,'0.########')"/>
								</PRICE>

								<xsl:variable name="varCommission">
									<xsl:value-of select="(OldCommission + OldSoftCommission)"/>
								</xsl:variable>

								<xsl:variable name="varCommission1">
									<xsl:choose>
										<xsl:when test="SettlCurrFxRate=0">
											<xsl:value-of select="$varCommission"/>
										</xsl:when>
										<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
											<xsl:value-of select="$varCommission * SettlCurrFxRate"/>
										</xsl:when>

										<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
											<xsl:value-of select="$varCommission div SettlCurrFxRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<COMMISSION>
									<xsl:value-of select="format-number($varCommission1,'0.###')"/>
								</COMMISSION>

								<STAMPTAX>
									<xsl:value-of select="''"/>
								</STAMPTAX>

								<xsl:variable name="varOldOtherFees">
									<xsl:value-of select="OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + TaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/>
								</xsl:variable>

								<xsl:variable name="varOtherFees1">
									<xsl:choose>
										<xsl:when test="SettlCurrFxRate=0">
											<xsl:value-of select="$varOldOtherFees"/>
										</xsl:when>
										<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
											<xsl:value-of select="$varOldOtherFees * SettlCurrFxRate"/>
										</xsl:when>

										<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
											<xsl:value-of select="$varOldOtherFees div SettlCurrFxRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<LOCALCHGS>
									<xsl:value-of select="''"/>
								</LOCALCHGS>

								<INTEREST>
									<xsl:value-of select="''"/>
								</INTEREST>

								<PRINCIPAL>
									<xsl:value-of select="''"/>
								</PRINCIPAL>

								<SECFEE>
									<xsl:value-of select="''"/>
								</SECFEE>

								<xsl:variable name="PB_NAME" select="''"/>
								<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
								<xsl:variable name="THIRDPARTY_BROKER">
									<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
								</xsl:variable>
								<EXECBROKER>
									<xsl:value-of select="''"/>
								</EXECBROKER>


								<BROKEROS>
									<xsl:value-of select="''"/>
								</BROKEROS>

								<TRAILERCD1>
									<xsl:choose>
										<xsl:when test="Asset='EquityOption'">
											<xsl:choose>
												<xsl:when test="OldSide='Buy' or OldSide='Buy to Open' or OldSide='Sell short' or OldSide='Sell to Open'">
													<xsl:value-of select="'O'"/>
												</xsl:when>
												<xsl:when test="OldSide='Sell' or OldSide='Sell to Close' or OldSide='Buy to Close'">
													<xsl:value-of select="'C'"/>
												</xsl:when>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>

									</xsl:choose>
								</TRAILERCD1>




								<TRAILERCD2>
									<xsl:value-of select="''"/>
								</TRAILERCD2>

								<TRAILERCD3>
									<xsl:choose>
										<xsl:when test=" OldSide='Sell short'">
											<xsl:value-of select="'S'"/>
										</xsl:when>
										<xsl:when test="OldSide='Sell' or OldSide='Sell to Close' or OldSide='Buy to Close'">
											<xsl:value-of select="''"/>
										</xsl:when>
									</xsl:choose>
								</TRAILERCD3>

								<BLOTTERCD>
									<xsl:value-of select="'49'"/>
								</BLOTTERCD>

								<CLRNGHSE>
									<xsl:value-of select="''"/>
								</CLRNGHSE>


								<CLRAGNTCD>
									<xsl:value-of select="$varCounterParty"/>
								</CLRAGNTCD>

								<CLRAGNT1>
									<xsl:value-of select="''"/>
								</CLRAGNT1>

								<CLRAGNT2>
									<xsl:value-of select="''"/>
								</CLRAGNT2>

								<CLRAGNT3>
									<xsl:value-of select="''"/>
								</CLRAGNT3>

								<CLRAGNT4>
									<xsl:value-of select="''"/>
								</CLRAGNT4>

								<CNTRPRTYCD>
									<xsl:value-of select="''"/>
								</CNTRPRTYCD>


								<CNTRPTY1>
									<xsl:value-of select="''"/>
								</CNTRPTY1>

								<CNTRPTY2>
									<xsl:value-of select="''"/>
								</CNTRPTY2>

								<CNTRPTY3>
									<xsl:value-of select="''"/>
								</CNTRPTY3>

								<CNTRPTY4>
									<xsl:value-of select="''"/>
								</CNTRPTY4>

								<INSTRUCT>
									<xsl:value-of select="''"/>
								</INSTRUCT>

								<CEDELAKV>
									<xsl:value-of select="''"/>
								</CEDELAKV>


								<ORIGLOCALREF>
									<xsl:value-of select="concat('PFJ',substring(EntityID,string-length(EntityID)-6,string-length(EntityID)))"/>
								</ORIGLOCALREF>

								<NOTES>
									<xsl:value-of select="''"/>
								</NOTES>

								<FILLER>
									<xsl:value-of select="''"/>
								</FILLER>

								<FILLER1>
									<xsl:value-of select="''"/>
								</FILLER1>

								<RR>
									<xsl:value-of select="''"/>
								</RR>

								<SETLCOUNTRYCD>
									<xsl:choose>
										<xsl:when test="contains(Asset,'EquityOption')">
											<xsl:value-of select="'US'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='USD'">
											<xsl:value-of select="'US'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='AED'">
											<xsl:value-of select="'AE'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='ARS'">
											<xsl:value-of select="'AR'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='BRL'">
											<xsl:value-of select="'BR'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='CAD'">
											<xsl:value-of select="'CA'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='CHF'">
											<xsl:value-of select="'CH'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='COP'">
											<xsl:value-of select="'CO'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='EUR'">
											<xsl:value-of select="'EU'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='GBP'">
											<xsl:value-of select="'GB'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='IDR'">
											<xsl:value-of select="'ID'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='INR'">
											<xsl:value-of select="'IN'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='JPY'">
											<xsl:value-of select="'JP'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='KES'">
											<xsl:value-of select="'KE'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='KRW'">
											<xsl:value-of select="'KR'"/>
										</xsl:when>

										<xsl:when test="CurrencySymbol='LKR'">
											<xsl:value-of select="'LK'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='MYR'">
											<xsl:value-of select="'MY'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='PEN'">
											<xsl:value-of select="'PE'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='PHP'">
											<xsl:value-of select="'PH'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='PLN'">
											<xsl:value-of select="'PL'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='RUB'">
											<xsl:value-of select="'RU'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='TZS'">
											<xsl:value-of select="'TZ'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='VND'">
											<xsl:value-of select="'VN'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='ZAR'">
											<xsl:value-of select="'ZA'"/>
										</xsl:when>

										<xsl:when test="CurrencySymbol='AUD'">
											<xsl:value-of select="'AU'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='BDT'">
											<xsl:value-of select="'BT'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</SETLCOUNTRYCD>

								<INSTRUMENTTYPE>
									<xsl:value-of select="''"/>
								</INSTRUMENTTYPE>


								<COMMISSIONRATE>
									<xsl:value-of select="''"/>
								</COMMISSIONRATE>

								<COMPANYNO>
									<xsl:value-of select="''"/>
								</COMPANYNO>

								<Filler2>
									<xsl:value-of select="''"/>
								</Filler2>

								<Filler3>
									<xsl:value-of select="''"/>
								</Filler3>

								<Filler4>
									<xsl:value-of select="''"/>
								</Filler4>

								<Filler5>
									<xsl:value-of select="''"/>
								</Filler5>

								<Filler6>
									<xsl:value-of select="''"/>
								</Filler6>


								<GPF2IDCode>
									<xsl:value-of select="''"/>
								</GPF2IDCode>


								<GPF2Amount>
									<xsl:value-of select="''"/>
								</GPF2Amount>

								<GPF2CurrencyCode>
									<xsl:value-of select="''"/>
								</GPF2CurrencyCode>

								<GPF2AddSubtract>
									<xsl:value-of select="''"/>
								</GPF2AddSubtract>



								<GPF3IDCode>
									<xsl:value-of select="''"/>
								</GPF3IDCode>

								<GPF3Amount>
									<xsl:value-of select="''"/>
								</GPF3Amount>

								<GPF3CurrencyCode>
									<xsl:value-of select="''"/>
								</GPF3CurrencyCode>

								<GPF3AddSubtract>
									<xsl:value-of select="''"/>
								</GPF3AddSubtract>

								<GPF4IDCode>
									<xsl:value-of select="''"/>
								</GPF4IDCode>

								<GPF4Amount>
									<xsl:value-of select="''"/>
								</GPF4Amount>

								<GPF4CurrencyCode>
									<xsl:value-of select="''"/>
								</GPF4CurrencyCode>

								<GPF4AddSubtract>
									<xsl:value-of select="''"/>
								</GPF4AddSubtract>

								<GPF5IDCode>
									<xsl:value-of select="''"/>
								</GPF5IDCode>

								<GPF5Amount>
									<xsl:value-of select="''"/>
								</GPF5Amount>

								<GPF5CurrencyCode>
									<xsl:value-of select="''"/>
								</GPF5CurrencyCode>

								<GPF5AddSubtract>
									<xsl:value-of select="''"/>
								</GPF5AddSubtract>

								<EntityID>
									<xsl:value-of select="EntityID"/>
								</EntityID>

							</ThirdPartyFlatFileDetail>
						</xsl:if>
						<ThirdPartyFlatFileDetail>

							<xsl:variable name="BLK">
								<xsl:call-template name="BLK">
									<xsl:with-param name="ID" select="PBUniqueID"/>
									<xsl:with-param name="Symbol" select="Symbol"/>
								</xsl:call-template>
							</xsl:variable>

							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>

							<FileHeader>
								<xsl:value-of select="'true'"/>
							</FileHeader>

							<FileFooter>
								<xsl:value-of select="'true'"/>
							</FileFooter


							<TaxLotState>
								<xsl:value-of select="'Allocated'"/>
							</TaxLotState>



							<LOCALREF>
								<xsl:value-of select="concat('PFJ',substring(AmendTaxLotId1,string-length(AmendTaxLotId1)-6,string-length(AmendTaxLotId1)))"/>
							</LOCALREF>

							<CFID>
								<xsl:value-of select="concat('PFJ',substring(AmendTaxLotId1,string-length(AmendTaxLotId1)-6,string-length(AmendTaxLotId1)))"/>
							</CFID>

							<ROUTECD>
								<xsl:value-of select="'PSHG'"/>
							</ROUTECD>

							<TIRORDERID>
								<xsl:value-of select="concat('BLK', $BLK, 'E')"/>
							</TIRORDERID>

							<TIRPIECE>
								<xsl:value-of select="''"/>
							</TIRPIECE>

							<TIRSEQ>
								<xsl:value-of select="''"/>
							</TIRSEQ>

							<SECIDTYPE>
								<xsl:choose>
									<xsl:when test="CurrencySymbol!='USD'">
										<xsl:choose>
											<xsl:when test="ISIN!=''">
												<xsl:value-of select="'N'"/>
											</xsl:when>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="CUSIP!=''">
												<xsl:value-of select="'C'"/>
											</xsl:when>
											<xsl:when test="SEDOL!=''">
												<xsl:value-of select="'D'"/>
											</xsl:when>
											<xsl:when test="Asset='Equity'">
												<xsl:value-of select="'S'"/>
											</xsl:when>
											<xsl:when test="contains(Asset,'Option')">
												<xsl:value-of select="'O'"/>
											</xsl:when>


										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>

							</SECIDTYPE>

							<SECURITYID>
								<xsl:choose>
									<xsl:when test="CurrencySymbol!='USD'">
										<xsl:choose>
											<xsl:when test="ISIN!=''">
												<xsl:value-of select="ISIN"/>
											</xsl:when>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="CUSIP!=''">
												<xsl:value-of select="'CUSIP'"/>
											</xsl:when>
											<xsl:when test="SEDOL!=''">
												<xsl:value-of select="SEDOL"/>
											</xsl:when>
											<xsl:when test="contains(Asset,'Option')">
												<xsl:value-of select="OSIOptionSymbol"/>
											</xsl:when>

											<xsl:when test="Asset='Equity'">
												<xsl:value-of select="Symbol"/>
											</xsl:when>
											<xsl:when test="Asset='FixedIncome'">
												<xsl:value-of select="CUSIP"/>
											</xsl:when>

										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>

							</SECURITYID>


							<xsl:variable name="varCounterParty">
								<xsl:value-of select="CounterParty"/>
							</xsl:variable>
							<DESCRIPTION1>
								<xsl:value-of select="concat('PB ',$varCounterParty)"/>
							</DESCRIPTION1>

							<DESCRIPTION2>
								<xsl:value-of select="''"/>
							</DESCRIPTION2>

							<DESCRIPTION3>
								<xsl:choose>
									<xsl:when test="Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'S*PERS'"/>
									</xsl:when>



									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>

								</xsl:choose>
							</DESCRIPTION3>

							<DESCRIPTION4>
								<xsl:value-of select="''"/>
							</DESCRIPTION4>

							<xsl:variable name="TradeDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="TradeDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="SettlementDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="SettlementDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="varMonthCode">
								<xsl:call-template name="MonthName">
									<xsl:with-param name="Month" select="substring-before($TradeDate,'/')"/>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="varSMonthCode">
								<xsl:call-template name="MonthName">
									<xsl:with-param name="Month" select="substring-before($SettlementDate,'/')"/>
								</xsl:call-template>
							</xsl:variable>

							<TRADEDATE>
								<xsl:value-of select="concat(substring-before(substring-after($TradeDate,'/'),'/'),'-',$varMonthCode,'-',substring(substring-after(substring-after($TradeDate,'/'),'/'),3,2))"/>
							</TRADEDATE>


							<SETLDATE>
								<xsl:value-of select="concat(substring-before(substring-after($SettlementDate,'/'),'/'),'-',$varSMonthCode,'-',substring(substring-after(substring-after($SettlementDate,'/'),'/'),3,2))"/>
							</SETLDATE>

							<QUANTITY>
								<xsl:value-of select="OrderQty"/>
							</QUANTITY>

							<QUANTITYDESC>
								<xsl:value-of select="''"/>
							</QUANTITYDESC>

							<NETMONEY>
								<xsl:value-of select="''"/>
							</NETMONEY>

							<xsl:variable name = "PRANA_FUND_NAME">
								<xsl:value-of select="''"/>
							</xsl:variable>

							<xsl:variable name ="THIRDPARTY_FUND_CODE">
								<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='']/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
							</xsl:variable>


							<xsl:variable name="varAccountName">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
										<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_FUND_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<CASHACCOUNT>
								<xsl:choose>
									<xsl:when test="contains(FundAccntNo,'PFJ004002')">
										<xsl:value-of select="'3DT8138710'"/>
									</xsl:when>
									<xsl:when test="contains(FundAccntNo,'PFJ001008')">
										<xsl:value-of select="'3DT3180460'"/>
									</xsl:when>
									<xsl:when test="contains(FundAccntNo,'PFJ002006')">
										<xsl:value-of select="'3DT3180460'"/>
									</xsl:when>
								</xsl:choose>
							</CASHACCOUNT>

							<SECACCOUNT>
								<xsl:choose>
									<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Sell' or Side='Sell to Close'">
										<xsl:value-of select="concat(FundAccntNo,'2')"/>
									</xsl:when>

									<xsl:when test="Side='Buy to Close' or Side='Buy to Cover' or Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="concat(FundAccntNo,'3')"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="FundAccntNo"/>
									</xsl:otherwise>

								</xsl:choose>
							</SECACCOUNT>

							<TRADECURRID>
								<xsl:value-of select="SettlCurrency"/>
							</TRADECURRID>

							<SETLCURRID>
								<xsl:value-of select="SettlCurrency"/>
							</SETLCURRID>

							<BSIND>
								<xsl:choose>
									<xsl:when test="contains(Side,'Buy')">
										<xsl:value-of select="'B'"/>
									</xsl:when>
									<xsl:when test="contains(Side,'Sell')">
										<xsl:value-of select="'S'"/>
									</xsl:when>
								</xsl:choose>
							</BSIND>


							<INSTTYP>
								<xsl:value-of select ="'N'"/>
							</INSTTYP>

							<xsl:variable name="AvgPrice1">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="AvgPrice"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="AvgPrice * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="AvgPrice div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<PRICE>
								<xsl:value-of select="format-number($AvgPrice1,'0.#######')"/>
							</PRICE>

							<xsl:variable name="varCommission">
								<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
							</xsl:variable>

							<xsl:variable name="varCommission1">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="$varCommission"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="$varCommission * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="$varCommission div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<COMMISSION>
								<xsl:value-of select="format-number($varCommission1,'0.##')"/>
							</COMMISSION>

							<STAMPTAX>
								<xsl:value-of select="''"/>
							</STAMPTAX>

							<xsl:variable name="varOtherFees">
								<xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
							</xsl:variable>

							<xsl:variable name="varOtherFees1">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="$varOtherFees"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="$varOtherFees * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="$varOtherFees div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<LOCALCHGS>
								<xsl:value-of select="''"/>
							</LOCALCHGS>

							<INTEREST>
								<xsl:value-of select="''"/>
							</INTEREST>

							<PRINCIPAL>
								<xsl:value-of select="''"/>
							</PRINCIPAL>

							<SECFEE>
								<xsl:value-of select="''"/>
							</SECFEE>

							<xsl:variable name="PB_NAME" select="''"/>
							<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
							<xsl:variable name="THIRDPARTY_BROKER">
								<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
							</xsl:variable>
							<EXECBROKER>
								<xsl:value-of select="''"/>
							</EXECBROKER>


							<BROKEROS>
								<xsl:value-of select="''"/>
							</BROKEROS>



							<TRAILERCD1>
								<xsl:choose>

									<xsl:when test="Asset='EquityOption'">
										<xsl:choose>
											<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Sell short' or Side='Sell to Open'">
												<xsl:value-of select="'O'"/>
											</xsl:when>
											<xsl:when test="Side='Sell' or Side='Sell to Close' or Side='Buy to Close'">
												<xsl:value-of select="'C'"/>
											</xsl:when>
										</xsl:choose>

									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>


							</TRAILERCD1>

							<TRAILERCD2>
								<xsl:value-of select="''"/>
							</TRAILERCD2>

							<TRAILERCD3>
								<xsl:choose>
									<xsl:when test=" Side='Sell short'">
										<xsl:value-of select="'S'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' or Side='Sell to Close' or Side='Buy to Close'">
										<xsl:value-of select="''"/>
									</xsl:when>
								</xsl:choose>
							</TRAILERCD3>

							<BLOTTERCD>
								<xsl:value-of select="'49'"/>
							</BLOTTERCD>

							<CLRNGHSE>
								<xsl:value-of select="''"/>
							</CLRNGHSE>


							<CLRAGNTCD>
								<xsl:value-of select="$varCounterParty"/>
							</CLRAGNTCD>

							<CLRAGNT1>
								<xsl:value-of select="''"/>
							</CLRAGNT1>

							<CLRAGNT2>
								<xsl:value-of select="''"/>
							</CLRAGNT2>

							<CLRAGNT3>
								<xsl:value-of select="''"/>
							</CLRAGNT3>

							<CLRAGNT4>
								<xsl:value-of select="''"/>
							</CLRAGNT4>

							<CNTRPRTYCD>
								<xsl:value-of select="''"/>
							</CNTRPRTYCD>


							<CNTRPTY1>
								<xsl:value-of select="''"/>
							</CNTRPTY1>

							<CNTRPTY2>
								<xsl:value-of select="''"/>
							</CNTRPTY2>

							<CNTRPTY3>
								<xsl:value-of select="''"/>
							</CNTRPTY3>

							<CNTRPTY4>
								<xsl:value-of select="''"/>
							</CNTRPTY4>

							<INSTRUCT>
								<xsl:value-of select="''"/>
							</INSTRUCT>

							<CEDELAKV>
								<xsl:value-of select="''"/>
							</CEDELAKV>


							<ORIGLOCALREF>
								<xsl:value-of select="concat('PFJ',substring(AmendTaxLotId1,string-length(AmendTaxLotId1)-6,string-length(AmendTaxLotId1)))"/>
							</ORIGLOCALREF>

							<NOTES>
								<xsl:value-of select="''"/>
							</NOTES>

							<FILLER>
								<xsl:value-of select="''"/>
							</FILLER>

							<FILLER1>
								<xsl:value-of select="''"/>
							</FILLER1>

							<RR>
								<xsl:value-of select="''"/>
							</RR>

							<SETLCOUNTRYCD>
								<xsl:choose>
									<xsl:when test="contains(Asset,'EquityOption')">
										<xsl:value-of select="'US'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='USD'">
										<xsl:value-of select="'US'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='AED'">
										<xsl:value-of select="'AE'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='ARS'">
										<xsl:value-of select="'AR'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='BRL'">
										<xsl:value-of select="'BR'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='CAD'">
										<xsl:value-of select="'CA'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='CHF'">
										<xsl:value-of select="'CH'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='COP'">
										<xsl:value-of select="'CO'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='EUR'">
										<xsl:value-of select="'EU'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='GBP'">
										<xsl:value-of select="'GB'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='IDR'">
										<xsl:value-of select="'ID'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='INR'">
										<xsl:value-of select="'IN'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='JPY'">
										<xsl:value-of select="'JP'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='KES'">
										<xsl:value-of select="'KE'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='KRW'">
										<xsl:value-of select="'KR'"/>
									</xsl:when>

									<xsl:when test="CurrencySymbol='LKR'">
										<xsl:value-of select="'LK'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='MYR'">
										<xsl:value-of select="'MY'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='PEN'">
										<xsl:value-of select="'PE'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='PHP'">
										<xsl:value-of select="'PH'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='PLN'">
										<xsl:value-of select="'PL'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='RUB'">
										<xsl:value-of select="'RU'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='TZS'">
										<xsl:value-of select="'TZ'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='VND'">
										<xsl:value-of select="'VN'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='ZAR'">
										<xsl:value-of select="'ZA'"/>
									</xsl:when>

									<xsl:when test="CurrencySymbol='AUD'">
										<xsl:value-of select="'AU'"/>
									</xsl:when>
									<xsl:when test="CurrencySymbol='BDT'">
										<xsl:value-of select="'BT'"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SETLCOUNTRYCD>

							<INSTRUMENTTYPE>
								<xsl:value-of select="''"/>
							</INSTRUMENTTYPE>


							<COMMISSIONRATE>
								<xsl:value-of select="''"/>
							</COMMISSIONRATE>

							<COMPANYNO>
								<xsl:value-of select="''"/>
							</COMPANYNO>



							<Filler2>
								<xsl:value-of select="''"/>
							</Filler2>

							<Filler3>
								<xsl:value-of select="''"/>
							</Filler3>

							<Filler4>
								<xsl:value-of select="''"/>
							</Filler4>


							<GPF2IDCode>
								<xsl:value-of select="''"/>
							</GPF2IDCode>


							<GPF2Amount>
								<xsl:value-of select="''"/>
							</GPF2Amount>

							<GPF2CurrencyCode>
								<xsl:value-of select="''"/>
							</GPF2CurrencyCode>

							<GPF2AddSubtract>
								<xsl:value-of select="''"/>
							</GPF2AddSubtract>



							<GPF3IDCode>
								<xsl:value-of select="''"/>
							</GPF3IDCode>

							<GPF3Amount>
								<xsl:value-of select="''"/>
							</GPF3Amount>

							<GPF3CurrencyCode>
								<xsl:value-of select="''"/>
							</GPF3CurrencyCode>

							<GPF3AddSubtract>
								<xsl:value-of select="''"/>
							</GPF3AddSubtract>

							<GPF4IDCode>
								<xsl:value-of select="''"/>
							</GPF4IDCode>

							<GPF4Amount>
								<xsl:value-of select="''"/>
							</GPF4Amount>

							<GPF4CurrencyCode>
								<xsl:value-of select="''"/>
							</GPF4CurrencyCode>

							<GPF4AddSubtract>
								<xsl:value-of select="''"/>
							</GPF4AddSubtract>

							<GPF5IDCode>
								<xsl:value-of select="''"/>
							</GPF5IDCode>

							<GPF5Amount>
								<xsl:value-of select="''"/>
							</GPF5Amount>

							<GPF5CurrencyCode>
								<xsl:value-of select="''"/>
							</GPF5CurrencyCode>

							<GPF5AddSubtract>
								<xsl:value-of select="''"/>
							</GPF5AddSubtract>
							<EntityID>
								<xsl:value-of select="EntityID"/>
							</EntityID>

						</ThirdPartyFlatFileDetail>

					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
