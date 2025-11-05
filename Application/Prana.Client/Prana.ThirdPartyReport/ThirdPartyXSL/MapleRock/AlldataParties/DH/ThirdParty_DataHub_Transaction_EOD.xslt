<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
	<xsl:output method="xml" indent="yes"/>

	<xsl:template match="/NewDataSet">

		<ThirdPartyFlatFileDetailCollection>
			<!--<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>-->

			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<!--<if	test="CounterParty !='CCMB' and not(contains(PRANA_FUND_NAME, 'KH'))">-->
				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>


					<Symbol>
						<xsl:value-of select="Symbol"/>
					</Symbol>


					<Quantity>
						<xsl:choose>
							<xsl:when test="number(AllocatedQty)">
								<xsl:value-of select="AllocatedQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Quantity>



					<AvragePrice>
						<xsl:choose>
							<xsl:when test="number(AvgPX)">
								<xsl:value-of select="AvgPX"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</AvragePrice>

					<Currency>
						<xsl:value-of select="CurrencySymbol"/>
					</Currency>

					<xsl:variable name="varORMonth">
						<xsl:value-of select="substring-before(substring-after(OriginalPurchaseDate,'-'),'-')"/>
					</xsl:variable>

					<xsl:variable name="varORDay">
						<xsl:value-of select="substring-before(substring-after(substring-after(OriginalPurchaseDate,'-'),'-'),'T')"/>
					</xsl:variable>

					<xsl:variable name="varORYear">
						<xsl:value-of select="substring-before(OriginalPurchaseDate,'-')"/>
					</xsl:variable>

					<OriginalPurchaseDate>
						<xsl:value-of select="concat($varORMonth,'/',$varORDay,'/',$varORYear)"/>
					</OriginalPurchaseDate>
					
					
					<xsl:variable name="varMonth">
						<xsl:value-of select="substring-before(substring-after(TradeDate,'-'),'-')"/>
					</xsl:variable>

					<xsl:variable name="varDay">
						<xsl:value-of select="substring-before(substring-after(substring-after(TradeDate,'-'),'-'),'T')"/>
					</xsl:variable>

					<xsl:variable name="varYear">
						<xsl:value-of select="substring-before(TradeDate,'-')"/>
					</xsl:variable>

					<Tradedate>
						<xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
					</Tradedate>


					<xsl:variable name="varPMonth">
						<xsl:value-of select="substring-before(substring-after(ProcessDate,'-'),'-')"/>
					</xsl:variable>

					<xsl:variable name="varPDay">
						<xsl:value-of select="substring-before(substring-after(substring-after(ProcessDate,'-'),'-'),'T')"/>
					</xsl:variable>

					<xsl:variable name="varPYear">
						<xsl:value-of select="substring-before(ProcessDate,'-')"/>
					</xsl:variable>

					<ProcessDate>
						<xsl:value-of select="concat($varPMonth,'/',$varPDay,'/',$varPYear)"/>
					</ProcessDate>
					
					<xsl:variable name="varMonth2">
						<xsl:value-of select="substring-before(substring-after(SettlementDate,'-'),'-')"/>
					</xsl:variable>

					<xsl:variable name="varDay2">
						<xsl:value-of select="substring-before(substring-after(substring-after(SettlementDate,'-'),'-'),'T')"/>
					</xsl:variable>

					<xsl:variable name="varYear2">
						<xsl:value-of select="substring-before(SettlementDate,'-')"/>
					</xsl:variable>
					<Settledate>
						<xsl:value-of select="concat($varMonth2,'/',$varDay2,'/',$varYear2)"/>

					</Settledate>


					<xsl:variable name="varEMonth">
						<xsl:value-of select="substring-before(substring-after(ExpirationDate,'-'),'-')"/>
					</xsl:variable>

					<xsl:variable name="varEDay">
						<xsl:value-of select="substring-before(substring-after(substring-after(ExpirationDate,'-'),'-'),'T')"/>
					</xsl:variable>

					<xsl:variable name="varEYear">
						<xsl:value-of select="substring-before(ExpirationDate,'-')"/>
					</xsl:variable>

					<ExpirationDate>
						<xsl:value-of select="concat($varEMonth,'/',$varEDay,'/',$varEYear)"/>
					</ExpirationDate>


					<Fxrate>
						<xsl:choose>
							<xsl:when test="number(FxRate)">
								<xsl:value-of select="FxRate"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="'1'"/>
							</xsl:otherwise>
						</xsl:choose>
					</Fxrate>

					<Description>
						<xsl:value-of select="CompanyName"/>
					</Description>


					<UnderlyingSymbol>
						<xsl:value-of select="UnderlyingSymbol"/>
					</UnderlyingSymbol>


					<xsl:variable name="PB_NAME">
						<xsl:value-of select="'BNP'"/>
					</xsl:variable>

					<xsl:variable name="PRANA_FUND_NAME" select="AccountName"/>

					<xsl:variable name="THIRDPARTY_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>

					<xsl:variable name="AccountId">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>


					<AccountName>
						<xsl:value-of select="$AccountId"/>
					</AccountName>

					<Commission>
						<xsl:value-of select="Commission"/>
					</Commission>


					<OtherBrokerFee>
						<xsl:choose>
							<xsl:when test="number(OtherBrokerFees)">
								<xsl:value-of select="OtherBrokerFees"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</OtherBrokerFee>


					<ClearingFees>
						<xsl:choose>
							<xsl:when test="number(ClearingFees)">
								<xsl:value-of select="ClearingFees"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</ClearingFees>

					<MiscFee>
						<xsl:choose>
							<xsl:when test="number(MiscFee)">
								<xsl:value-of select="MiscFee"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</MiscFee>


					<AUECFee1>
						<xsl:choose>
							<xsl:when test="number(AUECFee1)">
								<xsl:value-of select="AUECFee1"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</AUECFee1>

					<AUECFee2>
						<xsl:choose>
							<xsl:when test="number(AUECFee2)">
								<xsl:value-of select="AUECFee2"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</AUECFee2>


					<StampDuty>
						<xsl:choose>
							<xsl:when test="number(StampDuty)">
								<xsl:value-of select="StampDuty"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</StampDuty>

					<SEDOL>
						<xsl:value-of select="SEDOL"/>
					</SEDOL>

					<CUSIP>
						<xsl:value-of select="CUSIP"/>
					</CUSIP>

					<Bloomberg>
						<xsl:value-of select="Bloomberg"/>
					</Bloomberg>



					<Side>
						<xsl:value-of select="Side"/>
					</Side>


					<AssetType>
						<xsl:value-of select="Asset"/>
					</AssetType>



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

					<Executingbroker>
						<xsl:value-of select="$Broker"/>
					</Executingbroker>

					<GrossNotionalValue>
						<xsl:choose>
							<xsl:when test="number(GrossNotionalValue)">
								<xsl:value-of select="GrossNotionalValue"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</GrossNotionalValue>

					<GrossNotionalValueBase>
						<xsl:choose>
							<xsl:when test="number(GrossNotionalValueBase)">
								<xsl:value-of select="GrossNotionalValueBase"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</GrossNotionalValueBase>

					<NetNotionalValue>
						<xsl:choose>
							<xsl:when test="number(NetNotionalValue)">
								<xsl:value-of select="NetNotionalValue"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</NetNotionalValue>

					<NetNotionalValueBase>
						<xsl:choose>
							<xsl:when test="number(NetNotionalValueBase)">
								<xsl:value-of select="NetNotionalValueBase"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</NetNotionalValueBase>



					<TotalCommissionandFees>
						<xsl:choose>
							<xsl:when test="number(TotalCommissionandFees)">
								<xsl:value-of select="TotalCommissionandFees"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</TotalCommissionandFees>

					<TotalCommissionandFeesBase>
						<xsl:choose>
							<xsl:when test="number(TotalCommissionandFeesBase)">
								<xsl:value-of select="TotalCommissionandFeesBase"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</TotalCommissionandFeesBase>

					<SecFee>
						<xsl:choose>
							<xsl:when test="number(SecFee)">
								<xsl:value-of select="SecFee"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>


					</SecFee>

					<OccFee>
						<xsl:choose>
							<xsl:when test="number(OccFee)">
								<xsl:value-of select="OccFee"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</OccFee>

					<OrfFee>
						<xsl:choose>
							<xsl:when test="number(OrfFee)">
								<xsl:value-of select="OrfFee"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</OrfFee>

					<SecFeeBase>
						<xsl:choose>
							<xsl:when test="number(SecFeeBase)">
								<xsl:value-of select="SecFeeBase"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</SecFeeBase>


					<OccFeeBase>
						<xsl:choose>
							<xsl:when test="number(OccFeeBase)">
								<xsl:value-of select="OccFeeBase"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</OccFeeBase>


					<OrfFeeBase>
						<xsl:choose>
							<xsl:when test="number(OrfFeeBase)">
								<xsl:value-of select="OrfFeeBase"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</OrfFeeBase>

					<ClearingBrokerFeeBase>
						<xsl:choose>
							<xsl:when test="number(ClearingBrokerFeeBase)">
								<xsl:value-of select="ClearingBrokerFeeBase"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</ClearingBrokerFeeBase>



					<SoftCommission>
						<xsl:choose>
							<xsl:when test="number(SoftCommission)">
								<xsl:value-of select="SoftCommission"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</SoftCommission>

					<SoftCommissionBase>
						<xsl:choose>
							<xsl:when test="number(SoftCommissionBase)">
								<xsl:value-of select="SoftCommissionBase"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</SoftCommissionBase>

					<TaxOnCommissions>
						<xsl:choose>
							<xsl:when test="number(TaxOnCommissions)">
								<xsl:value-of select="TaxOnCommissions"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</TaxOnCommissions>

					<Transition>
						<xsl:value-of select="TransactionType"/>
					</Transition>

					<UnitCost>
						<xsl:choose>
							<xsl:when test="number(UnitCost)">
								<xsl:value-of select="UnitCost"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</UnitCost>


					<BaseCurrency>
						<xsl:value-of select="BaseCurrency"/>
					</BaseCurrency>

					<SettlCurrency>
						<xsl:value-of select="SettlCurrency"/>
					</SettlCurrency>

					<SettlFxRate>
						<xsl:choose>
							<xsl:when test="number(SettlFxRate)">
								<xsl:value-of select="SettlFxRate"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</SettlFxRate>

					<SettleCrrFxRate>
						<xsl:choose>
							<xsl:when test="number(SettleCrrFxRate)">
								<xsl:value-of select="SettleCrrFxRate"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</SettleCrrFxRate>


					<SettlPrice>
						<xsl:choose>
							<xsl:when test="number(SettlPrice)">
								<xsl:value-of select="SettlPrice"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					
					</SettlPrice>


					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>
</xsl:stylesheet>
