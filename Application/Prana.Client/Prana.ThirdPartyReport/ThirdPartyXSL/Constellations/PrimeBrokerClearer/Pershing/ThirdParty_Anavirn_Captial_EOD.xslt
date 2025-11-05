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
			

			<xsl:for-each select="ThirdPartyFlatFileDetail [FundAccntNo!='BYC896689']">

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
								<xsl:value-of select="concat('BYC',substring(EntityID,string-length(EntityID)-7,string-length(EntityID)))"/>
								<!--<xsl:value-of select="concat('BYC',PBUniqueID,'A')"/>-->
							</LOCALREF>

							<CFID>
								<!--<xsl:value-of select="concat('BYC',substring(EntityID,string-length(EntityID)-7,string-length(EntityID)))"/>-->
								<xsl:value-of select="concat('BYC',PBUniqueID,'A')"/>
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
											<xsl:when test="SEDOL!=''">
												<xsl:value-of select="'D'"/>
											</xsl:when>
											<xsl:when test="ISIN!=''">
												<xsl:value-of select="'I'"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="'S'"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="contains(Asset,'Option')">
												<xsl:value-of select="'O'"/>
											</xsl:when>


											<xsl:when test="Asset='Equity'">
												<xsl:value-of select="'S'"/>
											</xsl:when>

											<xsl:when test="CUSIP!=''">
												<xsl:value-of select="'C'"/>
											</xsl:when>
											<xsl:when test="ISIN!=''">
												<xsl:value-of select="'N'"/>
											</xsl:when>

										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>
							</SECIDTYPE>

							<SECURITYID>
								<xsl:choose>
									<xsl:when test="CurrencySymbol!='USD'">
										<xsl:choose>
											<xsl:when test="SEDOL!=''">
												<xsl:value-of select="SEDOL"/>
											</xsl:when>
											<xsl:when test="ISIN!=''">
												<xsl:value-of select="ISIN"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="Symbol"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>

											<xsl:when test="contains(Asset,'Option')">
												<xsl:value-of select="OSIOptionSymbol"/>
											</xsl:when>

											<xsl:when test="Asset='Equity'">
												<xsl:value-of select="Symbol"/>
											</xsl:when>

											<xsl:when test="Asset='FixedIncome'">
												<xsl:value-of select="CUSIP"/>
											</xsl:when>

											<xsl:when test="ISIN!=''">
												<xsl:value-of select="ISIN"/>
											</xsl:when>

										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>
							</SECURITYID>

							<xsl:variable name="varCounterParty">
								<xsl:value-of select="CounterParty"/>
							</xsl:variable>
							<DESCRIPTION1>
								<xsl:choose>
									<xsl:when test="CounterParty='PERS'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="concat('PB ',$varCounterParty)"/>
									</xsl:otherwise>
								</xsl:choose>
							</DESCRIPTION1>

							<DESCRIPTION2>
								<xsl:value-of select="''"/>
							</DESCRIPTION2>

							<DESCRIPTION3>
								<xsl:choose>
									<xsl:when test="Side='Sell short'">
										<xsl:value-of select="concat('S','*','PERS')"/>
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
								<xsl:value-of select="'3DT3188550'"/>
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
							
							<xsl:variable name="varFundAccountNo">
								<xsl:choose>
									<xsl:when test="FundAccntNo='787088'">
										<xsl:value-of select="'3NA1064230'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="FundAccntNo"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							
							<CASHACCOUNT>
								<xsl:value-of select="'3DT3188550'"/>
							</CASHACCOUNT>

							<SECACCOUNT>
								<xsl:choose>
									<xsl:when test="$varFundAccountNo='BYC003005' and (Side='Buy' or Side='Buy to Open' or Side='Sell' or Side='Sell to Close')">
										<xsl:value-of select="concat($varFundAccountNo,'2')"/>
									</xsl:when>

									<xsl:when test="$varFundAccountNo='BYC003005' and (Side='Buy to Close' or Side='Buy to Cover' or Side='Sell short' or Side='Sell to Open')">
										<xsl:value-of select="concat($varFundAccountNo,'3')"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="$varFundAccountNo"/>
									</xsl:otherwise>

								</xsl:choose>
							</SECACCOUNT>

							<TRADECURRID>
								<xsl:value-of select="CurrencySymbol"/>
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

							<xsl:variable name="varSettFxAmt">
								<xsl:choose>
									<xsl:when test="SettlCurrency != CurrencySymbol">
										<xsl:choose>
											<xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
												<xsl:value-of select="AvgPrice * FXRate_Taxlot"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="AvgPrice div FXRate_Taxlot"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="AvgPrice"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="varPrice">
								<xsl:choose>
									<xsl:when test="SettlCurrency = CurrencySymbol">
										<xsl:value-of select="AvgPrice"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$varSettFxAmt"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<PRICE>
								<xsl:value-of select="format-number($varPrice,'0.#######')"/>
							</PRICE>

							<xsl:variable name="varFXRate">
								<xsl:choose>
									<xsl:when test="SettlCurrency != CurrencySymbol">
										<xsl:value-of select="FXRate_Taxlot"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="varCommission">
								<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
							</xsl:variable>

							<xsl:variable name="varCommission1">
								<xsl:choose>
									<xsl:when test="$varFXRate=0">
										<xsl:value-of select="$varCommission"/>
									</xsl:when>
									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='M'">
										<xsl:value-of select="$varCommission * $varFXRate"/>
									</xsl:when>

									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='D'">
										<xsl:value-of select="$varCommission div $varFXRate"/>
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
								<xsl:value-of select="StampDuty"/>
							</STAMPTAX>

							<xsl:variable name="varOtherFees">
								<xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + OccFee + OrfFee"/>
							</xsl:variable>

							<xsl:variable name="varOtherFees1">
								<xsl:choose>
									<xsl:when test="$varFXRate=0">
										<xsl:value-of select="$varOtherFees"/>
									</xsl:when>
									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='M'">
										<xsl:value-of select="$varOtherFees * $varFXRate"/>
									</xsl:when>

									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='D'">
										<xsl:value-of select="$varOtherFees div $varFXRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<LOCALCHGS>
								<xsl:value-of select="$varOtherFees1"/>
							</LOCALCHGS>

							<INTEREST>
								<xsl:value-of select="AccruedInterest"/>
							</INTEREST>

							<PRINCIPAL>
								<xsl:value-of select="''"/>
							</PRINCIPAL>

							<SECFEE>
								<xsl:value-of select="SecFee"/>
							</SECFEE>


							<EXECBROKER>
								<xsl:choose>
									<xsl:when test="contains(Asset,'Option')">
										<xsl:value-of select="CounterParty"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</EXECBROKER>


							<BROKEROS>
								<xsl:value-of select="''"/>
							</BROKEROS>

							<TRAILERCD1>
								<xsl:value-of select="''"/>
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
								<xsl:value-of select="'Y'"/>
							</CLRNGHSE>


							<CLRAGNTCD>
								<xsl:value-of select="CounterParty"/>
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
								<xsl:value-of select="concat('BYC',PBUniqueID,'A')"/>
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
								<xsl:value-of select="substring(SettlCurrency,1,2)"/>
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
                  <xsl:value-of select="concat('BYC',substring(EntityID,string-length(EntityID)-7,string-length(EntityID)))"/>
									<!--<xsl:value-of select="concat('BYC',PBUniqueID,'A')"/>-->
								</LOCALREF>

								<CFID>
									<xsl:value-of select="concat('BYC',PBUniqueID,'A')"/>
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
												<xsl:when test="SEDOL!='' or SEDOL!='*'">
													<xsl:value-of select="'D'"/>
												</xsl:when>
												<xsl:when test="ISIN!=''">
													<xsl:value-of select="'I'"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="'S'"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="contains(Asset,'Option')">
													<xsl:value-of select="'O'"/>
												</xsl:when>


												<xsl:when test="Asset='Equity'">
													<xsl:value-of select="'S'"/>
												</xsl:when>

												<xsl:when test="CUSIP!=''">
													<xsl:value-of select="'C'"/>
												</xsl:when>
												<xsl:when test="ISIN!=''">
													<xsl:value-of select="'N'"/>
												</xsl:when>

											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</SECIDTYPE>

								<SECURITYID>
									<xsl:choose>
										<xsl:when test="CurrencySymbol!='USD'">
											<xsl:choose>
												<xsl:when test="SEDOL!='' or SEDOL!='*'">
													<xsl:value-of select="SEDOL"/>
												</xsl:when>
												<xsl:when test="ISIN!=''">
													<xsl:value-of select="ISIN"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="Symbol"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>

												<xsl:when test="contains(Asset,'Option')">
													<xsl:value-of select="OSIOptionSymbol"/>
												</xsl:when>

												<xsl:when test="Asset='Equity'">
													<xsl:value-of select="Symbol"/>
												</xsl:when>

												<xsl:when test="Asset='FixedIncome'">
													<xsl:value-of select="CUSIP"/>
												</xsl:when>

												<xsl:when test="ISIN!=''">
													<xsl:value-of select="ISIN"/>
												</xsl:when>

											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</SECURITYID>


								<xsl:variable name="varCounterParty">
									<xsl:value-of select="CounterParty"/>
								</xsl:variable>
								<DESCRIPTION1>
									<xsl:choose>
										<xsl:when test="CounterParty='PERS'">
											<xsl:value-of select="''"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat('PB ',$varCounterParty)"/>
										</xsl:otherwise>
									</xsl:choose>
								</DESCRIPTION1>

								<DESCRIPTION2>
									<xsl:value-of select="''"/>
								</DESCRIPTION2>

								<DESCRIPTION3>
									<xsl:choose>
										<xsl:when test="OldSide='Sell short'">
											<xsl:value-of select="concat('S','*',CounterParty)"/>
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
									<xsl:value-of select="'3DT3188550'"/>
								</CASHACCOUNT>
								
								<xsl:variable name="varFundAccountNo">
								<xsl:choose>
									<xsl:when test="FundAccntNo='787088'">
										<xsl:value-of select="'3NA1064230'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="FundAccntNo"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

								<SECACCOUNT>
									<xsl:choose>
										<xsl:when test="$varFundAccountNo='BYC003005' and (OldSide='Buy' or OldSide='Buy to Open' or OldSide='Sell' or OldSide='Sell to Close')">
											<xsl:value-of select="concat($varFundAccountNo,'1')"/>
										</xsl:when>

										<xsl:when test="$varFundAccountNo='BYC003005' and (OldSide='Buy to Close' or OldSide='Buy to Cover' or OldSide='Sell short' or OldSide='Sell to Open')">
											<xsl:value-of select="concat($varFundAccountNo,'3')"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="$varFundAccountNo"/>
										</xsl:otherwise>

									</xsl:choose>
								</SECACCOUNT>

								<TRADECURRID>
									<xsl:value-of select="CurrencySymbol"/>
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


								<xsl:variable name="varSettFxAmt">
									<xsl:choose>
										<xsl:when test="OldSettlCurrency != OldTransactionType">
											<xsl:choose>
												<xsl:when test="OldFXConversionMethodOperator ='M'">
													<xsl:value-of select="OldAvgPrice * OldFXRate"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="OldAvgPrice div OldFXRate"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="OldAvgPrice"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<xsl:variable name="varPrice">
									<xsl:choose>
										<xsl:when test="OldSettlCurrency = OldTransactionType">
											<xsl:value-of select="OldAvgPrice"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$varSettFxAmt"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<PRICE>
									<xsl:value-of select="format-number($varPrice,'0.########')"/>
								</PRICE>

								<xsl:variable name="varFXRate">
									<xsl:choose>
										<xsl:when test="OldSettlCurrency != OldTransactionType">
											<xsl:value-of select="OldFXRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<xsl:variable name="varCommission">
									<xsl:value-of select="(OldCommission + OldSoftCommission)"/>
								</xsl:variable>

								<xsl:variable name="varCommission1">
									<xsl:choose>
										<xsl:when test="$varFXRate=0">
											<xsl:value-of select="$varCommission"/>
										</xsl:when>
										<xsl:when test="$varFXRate!=0 and OldFXConversionMethodOperator='M'">
											<xsl:value-of select="$varCommission * $varFXRate"/>
										</xsl:when>

										<xsl:when test="$varFXRate!=0 and OldFXConversionMethodOperator='D'">
											<xsl:value-of select="$varCommission div $varFXRate"/>
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
									<xsl:value-of select="OldStampDuty"/>
								</STAMPTAX>

								<xsl:variable name="varOldOtherFees">
									<xsl:value-of select="OldOtherBrokerFees + OldClearingBrokerFee + OldTransactionLevy + OldClearingFee + TaxOnCommissions + OldMiscFees + OldOccFee + OldOrfFee"/>
								</xsl:variable>

								<xsl:variable name="varOtherFees1">
									<xsl:choose>
										<xsl:when test="$varFXRate=0">
											<xsl:value-of select="$varOldOtherFees"/>
										</xsl:when>
										<xsl:when test="$varFXRate!=0 and OldFXConversionMethodOperator='M'">
											<xsl:value-of select="$varOldOtherFees * $varFXRate"/>
										</xsl:when>

										<xsl:when test="$varFXRate!=0 and OldFXConversionMethodOperator='D'">
											<xsl:value-of select="$varOldOtherFees div $varFXRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<LOCALCHGS>
									<xsl:value-of select="$varOtherFees1"/>
								</LOCALCHGS>

								<INTEREST>
									<xsl:value-of select="OldAccruedInterest"/>
								</INTEREST>

								<PRINCIPAL>
									<xsl:value-of select="''"/>
								</PRINCIPAL>

								<SECFEE>
									<xsl:value-of select="OldSecFee"/>
								</SECFEE>


								<EXECBROKER>
									<xsl:choose>
										<xsl:when test="contains(Asset,'Option')">
											<xsl:value-of select="CounterParty"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</EXECBROKER>


								<BROKEROS>
									<xsl:value-of select="''"/>
								</BROKEROS>

								<TRAILERCD1>
									<xsl:value-of select="''"/>
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
									<xsl:value-of select="'Y'"/>
								</CLRNGHSE>


								<CLRAGNTCD>
									<xsl:value-of select="CounterParty"/>
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
									<xsl:value-of select="concat('BYC',PBUniqueID,'A')"/>
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
									<xsl:value-of select="substring(OldSettlCurrency,1,2)"/>
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
							</FileFooter>

							<TaxLotState>
								<xsl:value-of select="'Allocated'"/>
							</TaxLotState>


							<LOCALREF>
                <xsl:value-of select="concat('BYC',substring(EntityID,string-length(EntityID)-7,string-length(EntityID)))"/>
								<!--<xsl:value-of select="concat('BYC',PBUniqueID,'A')"/>-->
							</LOCALREF>

							<CFID>
								<xsl:value-of select="concat('BYC',PBUniqueID,'A')"/>
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
											<xsl:when test="SEDOL!=''">
												<xsl:value-of select="'D'"/>
											</xsl:when>
											<xsl:when test="ISIN!=''">
												<xsl:value-of select="'I'"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="'S'"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="contains(Asset,'Option')">
												<xsl:value-of select="'O'"/>
											</xsl:when>

											<xsl:when test="Asset='Equity'">
												<xsl:value-of select="'S'"/>
											</xsl:when>

											<xsl:when test="CUSIP!=''">
												<xsl:value-of select="'C'"/>
											</xsl:when>
											<xsl:when test="ISIN!=''">
												<xsl:value-of select="'N'"/>
											</xsl:when>

										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>
							</SECIDTYPE>

							<SECURITYID>
								<xsl:choose>
									<xsl:when test="CurrencySymbol!='USD'">
										<xsl:choose>
											<xsl:when test="SEDOL!=''">
												<xsl:value-of select="SEDOL"/>
											</xsl:when>
											<xsl:when test="ISIN!=''">
												<xsl:value-of select="ISIN"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="Symbol"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>

											<xsl:when test="contains(Asset,'Option')">
												<xsl:value-of select="OSIOptionSymbol"/>
											</xsl:when>

											<xsl:when test="Asset='Equity'">
												<xsl:value-of select="Symbol"/>
											</xsl:when>

											<xsl:when test="Asset='FixedIncome'">
												<xsl:value-of select="CUSIP"/>
											</xsl:when>

											<xsl:when test="ISIN!=''">
												<xsl:value-of select="ISIN"/>
											</xsl:when>

										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>
							</SECURITYID>

							<xsl:variable name="varCounterParty">
								<xsl:value-of select="CounterParty"/>
							</xsl:variable>
							<DESCRIPTION1>
								<xsl:choose>
									<xsl:when test="CounterParty='PERS'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="concat('PB ',$varCounterParty)"/>
									</xsl:otherwise>
								</xsl:choose>
							</DESCRIPTION1>

							<DESCRIPTION2>
								<xsl:value-of select="''"/>
							</DESCRIPTION2>

							<DESCRIPTION3>
								<xsl:choose>
									<xsl:when test="Side='Sell short'">
										<xsl:value-of select="concat('S','*',CounterParty)"/>
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
								<xsl:value-of select="'3DT3188550'"/>
							</CASHACCOUNT>
                             
							 <xsl:variable name="varFundAccountNo">
								<xsl:choose>
									<xsl:when test="FundAccntNo='787088'">
										<xsl:value-of select="'3NA1064230'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="FundAccntNo"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							 
							<SECACCOUNT>
								<xsl:choose>
									<xsl:when test="$varFundAccountNo='BYC003005' and (Side='Buy' or Side='Buy to Open' or Side='Sell' or Side='Sell to Close')">
										<xsl:value-of select="concat($varFundAccountNo,'2')"/>
									</xsl:when>

									<xsl:when test="$varFundAccountNo='BYC003005' and (Side='Buy to Close' or Side='Buy to Cover' or Side='Sell short' or Side='Sell to Open')">
										<xsl:value-of select="concat($varFundAccountNo,'3')"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="$varFundAccountNo"/>
									</xsl:otherwise>

								</xsl:choose>
							</SECACCOUNT>

							<TRADECURRID>
								<xsl:value-of select="CurrencySymbol"/>
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

							<xsl:variable name="varSettFxAmt">
								<xsl:choose>
									<xsl:when test="SettlCurrency != CurrencySymbol">
										<xsl:choose>
											<xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
												<xsl:value-of select="AvgPrice * FXRate_Taxlot"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="AvgPrice div FXRate_Taxlot"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="AvgPrice"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="varPrice">
								<xsl:choose>
									<xsl:when test="SettlCurrency = CurrencySymbol">
										<xsl:value-of select="AvgPrice"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$varSettFxAmt"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<PRICE>
								<xsl:value-of select="format-number($varPrice,'0.#######')"/>
							</PRICE>

							<xsl:variable name="varFXRate">
								<xsl:choose>
									<xsl:when test="SettlCurrency != CurrencySymbol">
										<xsl:value-of select="FXRate_Taxlot"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="varCommission">
								<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
							</xsl:variable>

							<xsl:variable name="varCommission1">
								<xsl:choose>
									<xsl:when test="$varFXRate=0">
										<xsl:value-of select="$varCommission"/>
									</xsl:when>
									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='M'">
										<xsl:value-of select="$varCommission * $varFXRate"/>
									</xsl:when>

									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='D'">
										<xsl:value-of select="$varCommission div $varFXRate"/>
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
								<xsl:value-of select="StampDuty"/>
							</STAMPTAX>

							<xsl:variable name="varOtherFees">
								<xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + OccFee + OrfFee"/>
							</xsl:variable>

							<xsl:variable name="varOtherFees1">
								<xsl:choose>
									<xsl:when test="$varFXRate=0">
										<xsl:value-of select="$varOtherFees"/>
									</xsl:when>
									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='M'">
										<xsl:value-of select="$varOtherFees * $varFXRate"/>
									</xsl:when>

									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='D'">
										<xsl:value-of select="$varOtherFees div $varFXRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<LOCALCHGS>
								<xsl:value-of select="$varOtherFees1"/>
							</LOCALCHGS>

							<INTEREST>
								<xsl:value-of select="AccruedInterest"/>
							</INTEREST>

							<PRINCIPAL>
								<xsl:value-of select="''"/>
							</PRINCIPAL>

							<SECFEE>
								<xsl:value-of select="SecFee"/>
							</SECFEE>


							<EXECBROKER>
								<xsl:choose>
									<xsl:when test="contains(Asset,'Option')">
										<xsl:value-of select="CounterParty"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</EXECBROKER>


							<BROKEROS>
								<xsl:value-of select="''"/>
							</BROKEROS>

							<TRAILERCD1>
								<xsl:value-of select="''"/>
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
								<xsl:value-of select="'Y'"/>
							</CLRNGHSE>


							<CLRAGNTCD>
								<xsl:value-of select="CounterParty"/>
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
								<xsl:value-of select="concat('BYC',PBUniqueID,'A')"/>
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
								<xsl:value-of select="substring(SettlCurrency,1,2)"/>
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

					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
