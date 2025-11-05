<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template name="GetMonth">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month = 1" >
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month = 2" >
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month = 3" >
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month = 4" >
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month = 5" >
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month = 6" >
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month = 7" >
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month = 8" >
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month = 9" >
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month = 10" >
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month = 11" >
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month = 12" >
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="/NewDataSet">
		<NewDataSet>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="ThirdPartyFlatFileDetail[not(contains(AccountName,'VABI'))]">
				<ThirdPartyFlatFileDetail>
					
					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name="FX">
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:value-of select="FXRate_Taxlot"/>
							</xsl:when>
							<!--<xsl:when test="number(ForexRate)">
								<xsl:value-of select="ForexRate"/>
							</xsl:when>-->
							<xsl:otherwise>
								<xsl:value-of select="ForexRate"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="PB_NAME" select="'MS'"/>

					<Currency_base>
						<xsl:value-of select="'USD'"/>
					</Currency_base>

					<Currency_local>
						<xsl:value-of select="CurrencySymbol"/>
					</Currency_local>

					<AssetClass>
						<xsl:choose>
							<xsl:when test="Asset='Equity' and IsSwapped='true'">
								<xsl:value-of select="'Equity Swap'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Asset"/>
							</xsl:otherwise>
						</xsl:choose>

					</AssetClass>

					<InvestementType>
						<xsl:choose>
							<xsl:when test="Asset='Equity' and IsSwapped='true'">
								<xsl:value-of select="'Equity Swap'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Asset"/>
							</xsl:otherwise>
						</xsl:choose>
					</InvestementType>

					<xsl:variable name="PRANA_BROKER_NAME" select="CounterParty"/>

					<xsl:variable name="PB_BROKER_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_BROKER_NAME]/@MLPBroker"/>
					</xsl:variable>

					<xsl:variable name="varBROKER">
						<xsl:choose>
							<xsl:when test="$PB_BROKER_NAME != ''">
								<xsl:value-of select="$PB_BROKER_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_BROKER_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<FXRate>
						<xsl:value-of select="$FX"/>
					</FXRate>

					<Symbol>
						<xsl:choose>
						
							<xsl:when test="Asset='EquityOption'">
								<xsl:choose>
									<xsl:when test="OSIOptionSymbol!='*'">
										<xsl:value-of select="OSIOptionSymbol"/>
									</xsl:when>
									<xsl:when test="BloombergSymbol!='*'">
										<xsl:value-of select="BloombergSymbol"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="Symbol"/>
									</xsl:otherwise>
								</xsl:choose>

							</xsl:when>
							
							
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</Symbol>

					<UnderlyingSymbol>
						<xsl:value-of select="UnderlyingSymbol"/>							
					</UnderlyingSymbol>

					<Description>
						<xsl:value-of select="translate(FullSecurityName,',','')"/>
					</Description>

					<TradeDate>
						<xsl:value-of select="TradeDate"/>
					</TradeDate>

					<!--<TradeTime>
						<xsl:value-of select="substring-after(TradeDateTime,' ')"/>
					</TradeTime>-->

					<xsl:variable name ="GrossAmount" select="AllocatedQty * AvgPrice * Multiplier"/>

					<xsl:variable name="GrossProceeds_Base">
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:choose>
									<xsl:when test="FXConversionMethodOperator='M'">
										<xsl:value-of select="$GrossAmount * $FX"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$GrossAmount div $FX"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:when test="number(ForexRate)">
								<xsl:choose>
									<xsl:when test="CurrencySymbol='GBP' or CurrencySymbol='AUD' or CurrencySymbol='EUR'">
										<xsl:value-of select="$GrossAmount * $FX"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$GrossAmount div $FX"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$GrossAmount"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					

					<Quantity>
						<xsl:choose>
							<xsl:when test="number(AllocatedQty)">
								<xsl:value-of select="AllocatedQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
						<!--<xsl:choose>
							<xsl:when test="LeadCurrencyName != 'USD'">
								<xsl:choose>
									<xsl:when test="GrossAmount &gt; 0">
										<xsl:value-of select="format-number(GrossAmount,'#.##')"/>
									</xsl:when>
									<xsl:when test="GrossAmount &lt; 0">
										<xsl:value-of select="format-number(GrossAmount * (-1),'#.##')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="number($GrossProceeds_Base)">
										<xsl:value-of select="format-number($GrossProceeds_Base,'#.##')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>-->
					</Quantity>

					<xsl:variable name="PRANA_FUND_NAME" select="AccountName"/>

					<xsl:variable name="THIRDPARTY_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>

					<ClientAccountID>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</ClientAccountID>

					<!--<xsl:variable name="GrossBase">
						<xsl:value-of select="AllocatedQty * $FX"/>
					</xsl:variable>-->



					<GrossProceeds_base>

						<xsl:choose>
							<xsl:when test="number($GrossProceeds_Base)">
								<xsl:value-of select="format-number($GrossProceeds_Base,'#.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

						<!--<xsl:choose>
							<xsl:when test="number($GrossBase)">
								<xsl:value-of select="$GrossBase"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>-->

					</GrossProceeds_base>

					<xsl:variable name="Commission_Base">
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:choose>
									<xsl:when test="FXConversionMethodOperator='M'">
										<xsl:value-of select="CommissionCharged * $FX"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="CommissionCharged div $FX"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:when test="number(ForexRate)">
								<xsl:choose>
									<xsl:when test="CurrencySymbol='GBP' or CurrencySymbol='AUD' or CurrencySymbol='EUR'">
										<xsl:value-of select="CommissionCharged * $FX"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="CommissionCharged div $FX"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="CommissionCharged"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<Commission_base>
						<xsl:choose>
							<xsl:when test="number($Commission_Base)">
								<xsl:value-of select="format-number($Commission_Base,'#.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>


						<!--<xsl:choose>
							<xsl:when test="FXConversionMethodOperator='M'">
								<xsl:value-of select="CommissionCharged * $FX"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="CommissionCharged div $FX"/>
							</xsl:otherwise>
						</xsl:choose>-->
					</Commission_base>

					<xsl:variable name="OtherFee">
						<xsl:value-of select="OtherBrokerFee + ClearingBrokerFee  + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees  + OccFee + OrfFee + StampDuty + SecFee"/>
					</xsl:variable>

					<xsl:variable name="OtherFees_Base">
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:choose>
									<xsl:when test="FXConversionMethodOperator='M'">
										<xsl:value-of select="$OtherFee * $FX"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$OtherFee div $FX"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:when test="number(ForexRate)">
								<xsl:choose>
									<xsl:when test="CurrencySymbol='GBP' or CurrencySymbol='AUD' or CurrencySymbol='EUR'">
										<xsl:value-of select="$OtherFee * $FX"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$OtherFee div $FX"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$OtherFee"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<OtherFees_base>

						<xsl:choose>
							<xsl:when test="number($OtherFees_Base)">
								<xsl:value-of select="format-number($OtherFees_Base,'#.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
						
						<!--<xsl:choose>
							<xsl:when test="FXConversionMethodOperator='M'">
								<xsl:value-of select="$OtherFee * $FX"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$OtherFee div $FX"/>
							</xsl:otherwise>
						</xsl:choose>-->
					</OtherFees_base>

					<xsl:variable name="NetAmount">
						<xsl:choose>
							<xsl:when test="contains(Side,'Buy')">
								<xsl:value-of select="$GrossAmount + $OtherFee + (CommissionCharged + SoftCommissionCharged)"/>
							</xsl:when>
							<xsl:when test="contains(Side,'Sell')">
								<xsl:value-of select="$GrossAmount - $OtherFee - (CommissionCharged + SoftCommissionCharged)"/>
							</xsl:when>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="NetProceeds_Base">
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:choose>
									<xsl:when test="FXConversionMethodOperator='M'">
										<xsl:value-of select="$NetAmount * $FX"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$NetAmount div $FX"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:when test="number(ForexRate)">
								<xsl:choose>
									<xsl:when test="CurrencySymbol='GBP' or CurrencySymbol='AUD' or CurrencySymbol='EUR'">
										<xsl:value-of select="$NetAmount * $FX"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$NetAmount div $FX"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$NetAmount"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<NetProceeds_base>
						
						<xsl:choose>
							<xsl:when test="number($NetProceeds_Base)">
								<xsl:value-of select="format-number($NetProceeds_Base,'#.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
						
						<!--<xsl:choose>
							<xsl:when test="FXConversionMethodOperator='M'">
								<xsl:value-of select="NetAmount * $FX"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="NetAmount div $FX"/>
							</xsl:otherwise>
						</xsl:choose>-->
					</NetProceeds_base>

					<GrossProceeds_local>
						<xsl:choose>
							<xsl:when test="$GrossAmount &gt; 0">
								<xsl:value-of select="format-number($GrossAmount,'#.##')"/>
							</xsl:when>
							<xsl:when test="$GrossAmount &lt; 0">
								<xsl:value-of select="format-number($GrossAmount * (-1),'#.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</GrossProceeds_local>

					<Commission_local>
						<xsl:choose>
							<xsl:when test="CommissionCharged &gt; 0">
								<xsl:value-of select="format-number(CommissionCharged,'#.##')"/>
							</xsl:when>
							<xsl:when test="CommissionCharged &lt; 0">
								<xsl:value-of select="format-number(CCommissionCharged * (-1),'#.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Commission_local>

					<OtherFees_local>
						<xsl:choose>
							<xsl:when test="$OtherFee &gt; 0">
								<xsl:value-of select="format-number($OtherFee,'#.##')"/>
							</xsl:when>
							<xsl:when test="$OtherFee &lt; 0">
								<xsl:value-of select="format-number($OtherFee * (-1),'#.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</OtherFees_local>

					<NetProceeds_local>
						<xsl:choose>
							<xsl:when test="$NetAmount &gt; 0">
								<xsl:value-of select="format-number($NetAmount,'#.##')"/>
							</xsl:when>
							<xsl:when test="$NetAmount &lt; 0">
								<xsl:value-of select="format-number($NetAmount * (-1),'#.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</NetProceeds_local>

					<TransactionType>
						<xsl:value-of select="TransactionType"/>
					</TransactionType>

					<Side>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:choose>
									<xsl:when test="Side='Buy' or Side='Buy to Open'">
										<xsl:value-of select="'Long'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
										<xsl:value-of select="'Short'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' or Side='Sell to Close'">
										<xsl:value-of select="'Long'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'Short'"/>
									</xsl:when>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="Side='Buy' or Side='Buy to Open'">
										<xsl:value-of select="'Long'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
										<xsl:value-of select="'Short'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' or Side='Sell to Close'">
										<xsl:value-of select="'Long'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'Short'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
								
							</xsl:otherwise>
							
						</xsl:choose>
					</Side>

					<PutCall>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="PutOrCall"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</PutCall>

					<Multiplier>
						<xsl:choose>
							<xsl:when test="number(Multiplier)">
								<xsl:value-of select="Multiplier"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="1"/>
							</xsl:otherwise>
						</xsl:choose>
					</Multiplier>

					<Strike>
						<xsl:choose>
							<xsl:when test="contains(Asset,'Option')">
								<xsl:value-of select="StrikePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					
					</Strike>

					<Expiration>
						<xsl:choose>
							<xsl:when test="contains(Asset,'Option')">
								<xsl:value-of select="ExpirationDate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</Expiration>

					<CUSIP>
						
						<xsl:value-of select="CUSIP"/>
					</CUSIP>

					<ISIN>
						
						<xsl:value-of select="ISIN"/>
					</ISIN>

					<Sedol>
						
						<xsl:value-of select="SEDOL"/>
					</Sedol>

					<!--<UnderlyingCUSIP>
						<xsl:value-of select="CUSIP"/>
					</UnderlyingCUSIP>

					<UnderlyingISIN>
						<xsl:value-of select="ISIN"/>
					</UnderlyingISIN>

					<UnderlyingSedol>

						<xsl:value-of select="SEDOL"/>
							
					</UnderlyingSedol>-->


					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				
			</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</NewDataSet>
	</xsl:template>
</xsl:stylesheet>
