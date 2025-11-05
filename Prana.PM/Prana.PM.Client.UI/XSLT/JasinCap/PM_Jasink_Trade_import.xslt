<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>

		<!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
	</xsl:template>

	<xsl:template match="/DocumentElement">

		<DocumentElement>

			<xsl:for-each select="//PositionMaster">

				<xsl:if test="number(COL16)">

					<PositionMaster>
						<xsl:variable name="varPBName">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>


						<!--<xsl:variable name="PRANA_STRATEGY_NAME">
							<xsl:value-of select="document('../ReconMappingXml/StrategyMapping.xml')/StrategyMapping/PB[@Name=$varPBName]/StrategyData[@PranaFundName=$PRANA_FUND_NAME]/@PranaStrategy"/>
						</xsl:variable>


						<Strategy>
							<xsl:value-of select ="$PRANA_STRATEGY_NAME"/>
						</Strategy>-->




						<FundName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="COL1"/>
								</xsl:otherwise>
							</xsl:choose>
						</FundName>

						<!--<xsl:variable name="varPB_Name">
							<xsl:value-of select="'Jefferies'"/>
						</xsl:variable>-->

						<xsl:variable name = "PB_COMPANY" >
							<xsl:value-of select="normalize-space(COL5)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
						</xsl:variable>

						<!--<FundName>
							<xsl:value-of select='"Jasinkiewicz"'/>
						</FundName>-->

						<!--<xsl:choose>
							<xsl:when test ="COL14 &lt; 0">
								<SideTagValue>
									<xsl:value-of select="'2'"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="COL14*(-1)"/>
								</NetPosition>
							</xsl:when>
							<xsl:when test ="COL14 &gt; 0">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="COL14"/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<SideTagValue>
									<xsl:value-of select="''"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>-->

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="contains(COL25,'OPT')">
									<xsl:choose>
										<xsl:when test="COL6='Buy'">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<xsl:when test="COL6='Sell'">
											<xsl:value-of select="'D'"/>
										</xsl:when>
										<xsl:when test="COL6='Sell Short'">
											<xsl:value-of select="'C'"/>
										</xsl:when>
										<xsl:when test="COL6='Cover Short'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="COL6='Buy'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="COL6='Sell'">
											<xsl:value-of select="'2'"/>
										</xsl:when>
										<xsl:when test="COL6='Sell Short'">
											<xsl:value-of select="'5'"/>
										</xsl:when>
										<xsl:when test="COL6='Cover Short'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>


						<NetPosition>
							<xsl:choose>
								<xsl:when test="number(COL16) &gt;0">
									<xsl:value-of select ="COL16"/>
								</xsl:when>
								<xsl:when test="number(COL16) &lt;0">
									<xsl:value-of select ="COL16*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition> 

						<!--<xsl:variable name ="varGrossAmt">
							<xsl:choose>
								<xsl:when test ="number(COL20) &lt; 0">
									<xsl:value-of select="number(COL20)*(-1) - (number(COL15) + number(COL16) + number(COL17)) "/>
								</xsl:when>
								<xsl:when test ="number(COL20) &gt; 0">
									<xsl:value-of select="COL20 + (number(COL15) + number(COL16) + number(COL17))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="COL14 &lt; 0 ">
								<CostBasis>
									<xsl:value-of select="$varGrossAmt div COL14 *(-1)"/>
								</CostBasis>
							</xsl:when>
							<xsl:when test ="COL14 &gt; 0 ">
								<CostBasis>
									<xsl:value-of select="$varGrossAmt div COL14 "/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>-->

						<CostBasis>
							<xsl:choose>
								<xsl:when test="number(COL14) &gt;0">
									<xsl:value-of select ="COL14"/>
								</xsl:when>
								<xsl:when test="number(COL14) &lt;0">
									<xsl:value-of select ="COL14*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<PBSymbol>
							<xsl:value-of select="normalize-space(COL5)"/>
						</PBSymbol>

						<Symbol>
							<xsl:choose>
								<xsl:when test ="$PRANA_SYMBOL != ''">
									<xsl:value-of select="$PRANA_SYMBOL"/>
								</xsl:when>
								<xsl:when test ="string-length(COL8) = 21">

									<xsl:value-of select ="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="normalize-space(COL10)"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol >

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test ="$PRANA_SYMBOL != ''">
									<xsl:value-of select="$PRANA_SYMBOL"/>
								</xsl:when>
								<xsl:when test ="string-length(COL8) = 21">
									<xsl:value-of select ="concat(COL8,'U')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</IDCOOptionSymbol>

						<PositionStartDate>
							<xsl:value-of select ="COL2"/>
						</PositionStartDate>

						<xsl:variable name="varCommission">
							<xsl:value-of select="COL17"/>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test="$varCommission &gt; 0">
									<xsl:value-of select="$varCommission"/>
								</xsl:when>
								<xsl:when test="$varCommission &lt; 0">
									<xsl:value-of select="$varCommission*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>


						<xsl:variable name="varFees">
							<xsl:value-of select="COL19"/>
						</xsl:variable>

						<Fees>
							<xsl:choose>
								<xsl:when test="$varFees &gt; 0">
									<xsl:value-of select="$varFees"/>
								</xsl:when>
								<xsl:when test="$varFees &lt; 0">
									<xsl:value-of select="$varFees*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>


						<xsl:variable name="varStampDuty">
							<xsl:value-of select="COL18"/>
						</xsl:variable>

						<StampDuty>
							<xsl:choose>
								<xsl:when test="$varStampDuty &gt; 0">
									<xsl:value-of select="$varStampDuty"/>
								</xsl:when>
								<xsl:when test="$varStampDuty &lt; 0">
									<xsl:value-of select="$varStampDuty*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</StampDuty>

						<xsl:variable name="TaxOnCommissions">
							<xsl:value-of select="number(COL27)"/>
						</xsl:variable>

						<TaxOnCommissions>
							<xsl:choose>
								<xsl:when test="$TaxOnCommissions &gt; 0">
									<xsl:value-of select="$TaxOnCommissions"/>
								</xsl:when>
								<xsl:when test="$TaxOnCommissions &lt; 0">
									<xsl:value-of select="$TaxOnCommissions*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TaxOnCommissions>

						<xsl:variable name="ClearingFee">
							<xsl:value-of select="number(COL28)"/>
						</xsl:variable>

						<ClearingFee>
							<xsl:choose>
								<xsl:when test="$ClearingFee &gt; 0">
									<xsl:value-of select="$ClearingFee"/>
								</xsl:when>
								<xsl:when test="$ClearingFee &lt; 0">
									<xsl:value-of select="$ClearingFee*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</ClearingFee>

						<xsl:variable name="TransactionLevy">
							<xsl:value-of select="number(COL29)"/>
						</xsl:variable>

						<TransactionLevy>
							<xsl:choose>
								<xsl:when test="$TransactionLevy &gt; 0">
									<xsl:value-of select="$TransactionLevy"/>
								</xsl:when>
								<xsl:when test="$TransactionLevy &lt; 0">
									<xsl:value-of select="$TransactionLevy*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TransactionLevy>

						<xsl:variable name="MiscFees">
							<xsl:value-of select="number(COL30)"/>
						</xsl:variable>

						<MiscFees>
							<xsl:choose>
								<xsl:when test="$MiscFees &gt; 0">
									<xsl:value-of select="$MiscFees"/>
								</xsl:when>
								<xsl:when test="$MiscFees &lt; 0">
									<xsl:value-of select="$MiscFees*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MiscFees>

						<xsl:variable name="SoftCommission">
							<xsl:value-of select="number(COL31)"/>
						</xsl:variable>

						<SoftCommission>
							<xsl:choose>
								<xsl:when test="$SoftCommission &gt; 0">
									<xsl:value-of select="$SoftCommission"/>
								</xsl:when>
								<xsl:when test="$SoftCommission &lt; 0">
									<xsl:value-of select="$SoftCommission*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SoftCommission>

						<xsl:variable name="OccFee">
							<xsl:value-of select="number(COL32)"/>
						</xsl:variable>

						<OccFee>
							<xsl:choose>
								<xsl:when test="$OccFee &gt; 0">
									<xsl:value-of select="$OccFee"/>
								</xsl:when>
								<xsl:when test="$OccFee &lt; 0">
									<xsl:value-of select="$OccFee*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</OccFee>

						<xsl:variable name="OrfFee">
							<xsl:value-of select="number(COL33)"/>
						</xsl:variable>

						<OrfFee>
							<xsl:choose>
								<xsl:when test="$OrfFee &gt; 0">
									<xsl:value-of select="$OrfFee"/>
								</xsl:when>
								<xsl:when test="$OrfFee &lt; 0">
									<xsl:value-of select="$OrfFee*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</OrfFee>

						<xsl:variable name="ClearingBrokerFee">
							<xsl:value-of select="number(COL34)"/>
						</xsl:variable>

						<ClearingBrokerFee>
							<xsl:choose>
								<xsl:when test="$ClearingBrokerFee &gt; 0">
									<xsl:value-of select="$ClearingBrokerFee"/>
								</xsl:when>
								<xsl:when test="$ClearingBrokerFee &lt; 0">
									<xsl:value-of select="$ClearingBrokerFee*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</ClearingBrokerFee>

						<xsl:variable name="OtherBrokerFees">
							<xsl:value-of select="number(COL35)"/>
						</xsl:variable>

						<OtherBrokerFees>
							<xsl:choose>
								<xsl:when test="$OtherBrokerFees &gt; 0">
									<xsl:value-of select="$OtherBrokerFees"/>
								</xsl:when>
								<xsl:when test="$OtherBrokerFees &lt; 0">
									<xsl:value-of select="$OtherBrokerFees*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</OtherBrokerFees>

						<xsl:variable name="SecFee">
							<xsl:value-of select="number(COL36)"/>
						</xsl:variable>
						
						<SecFee>
							<xsl:choose>
								<xsl:when test="$SecFee &gt; 0">
									<xsl:value-of select="$SecFee"/>
								</xsl:when>
								<xsl:when test="$SecFee &lt; 0">
									<xsl:value-of select="$SecFee*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecFee>

						<xsl:variable name="COL38">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL38"/>
							</xsl:call-template>
						</xsl:variable>


						<OptionPremiumAdjustment>
							<xsl:value-of select="$COL38"/>
						</OptionPremiumAdjustment>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>