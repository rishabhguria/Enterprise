<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:variable name = "PB_NAME">
		<xsl:value-of select="'QUAD'"/>
	</xsl:variable>


	<xsl:template name="FutureSymbol">

		<xsl:param name="varSymbol"/>

		<xsl:variable name="varUnderlyingFUTLME">
			<xsl:value-of select="substring($varSymbol,3,3)"/>
		</xsl:variable>

		<xsl:variable name="varUnderlyingFUTOption">
			<xsl:value-of select="substring($varSymbol,1,2)"/>
		</xsl:variable>

		<xsl:variable name="varYear" >
			<xsl:value-of select="substring($varSymbol,4,1)"/>
		</xsl:variable>

		<xsl:variable name="varMonthCode">
			<xsl:value-of select="substring($varSymbol,3,1)"/>
		</xsl:variable>

		<xsl:variable name="varDate" select="substring(substring-after($varSymbol,' '),7,2)"/>

		<xsl:variable name="varStrikePrice" select="substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(COL3,' '),' '),' '),' '),' '),' ')"/>

		<xsl:variable name="PB_CODE" select="normalize-space(substring($varSymbol,1,2))"/>

		<!--<xsl:variable name="PB_FLAG" select="normalize-space(translate(substring-after(substring-after(COL5,' '),' '),$varSmall,$varCapital))"/>-->

		<xsl:variable name="PRANA_UNDERLYING_NAME">
			<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_CODE]/@UnderlyingCode"/>
		</xsl:variable>

		<xsl:variable name="PRANA_EXCHANGE_NAME">
			<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_CODE]/@ExchangeCode"/>
		</xsl:variable>

		<xsl:variable name="PRANA_FLAG">
			<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_CODE]/@ExpFlag"/>
		</xsl:variable>

		<xsl:variable name="PRANA_STRIKE_MULTIPLIER">
			<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_CODE]/@StrikeMul"/>
		</xsl:variable>

		<xsl:variable name="StrikePrice">
			<xsl:choose>
				<xsl:when test="$PRANA_STRIKE_MULTIPLIER!=''">
					<xsl:value-of select="$varStrikePrice*$PRANA_STRIKE_MULTIPLIER"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$varStrikePrice"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varFUTOPTUnderlying">
			<xsl:choose>
				<xsl:when test="$PRANA_UNDERLYING_NAME!=''">
					<xsl:value-of select="$PRANA_UNDERLYING_NAME"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$varUnderlyingFUTOption"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varFUTLMEUnderlying">
			<xsl:choose>
				<xsl:when test="$PRANA_UNDERLYING_NAME!=''">
					<xsl:value-of select="$PRANA_UNDERLYING_NAME"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$varUnderlyingFUTLME"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varFUTUnderlying">
			<xsl:choose>
				<xsl:when test="$PRANA_UNDERLYING_NAME!=''">
					<xsl:value-of select="$PRANA_UNDERLYING_NAME"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring($varSymbol,1,2)"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="putCall" select="substring($varSymbol,5,1)"/>

	
		
		<xsl:choose>		

			<xsl:when test="$PRANA_FLAG=''">
				<xsl:choose>
					<xsl:when test="string-length($varSymbol)=4">
						<xsl:value-of select="normalize-space(concat($varFUTUnderlying,' ',$varMonthCode,$varYear,$PRANA_EXCHANGE_NAME))"/>
						<!--<xsl:choose>

							<xsl:when test="not(contains(COL8,'LME'))">
								<xsl:value-of select="normalize-space(concat($varFUTUnderlying,' ',$varMonthCode,$varYear,$PRANA_EXCHANGE_NAME))"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="concat($varFUTLMEUnderlying,' ',$varYear,$varMonthCode,$varDate,'-LME')"/>
							</xsl:otherwise>

						</xsl:choose>-->

					</xsl:when>

					<xsl:when test="string-length($varSymbol)=5">					
						<xsl:value-of select="concat($varFUTOPTUnderlying,' ',$varMonthCode,$varYear,$putCall,$StrikePrice,$PRANA_EXCHANGE_NAME)"/>
					</xsl:when>

				</xsl:choose>
			</xsl:when>

			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="string-length($varSymbol)=4">
						<xsl:value-of select="normalize-space(concat($varFUTUnderlying,' ',$varYear,$varMonthCode,$PRANA_EXCHANGE_NAME))"/>
						<!--<xsl:choose>

							<xsl:when test="not(contains(COL8,'LME'))">
								<xsl:value-of select="normalize-space(concat($varFUTUnderlying,' ',$varYear,$varMonthCode,$PRANA_EXCHANGE_NAME))"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="concat($varFUTLMEUnderlying,' ',$varYear,$varMonthCode,$varDate,'-LME')"/>
							</xsl:otherwise>

						</xsl:choose>-->

					</xsl:when>

					<xsl:when test="string-length($varSymbol)=5">
						<xsl:value-of select="concat($varFUTOPTUnderlying,' ',$varYear,$varMonthCode,$putCall,$StrikePrice,$PRANA_EXCHANGE_NAME)"/>
					</xsl:when>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">
				<xsl:if test ="number(COL6) or number(COL7)">
					<Comparision>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL3"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
						<xsl:variable name="Symbol">
							<xsl:call-template name="FutureSymbol">
								<xsl:with-param name="varSymbol" select="COL22"/>
							</xsl:call-template>
						</xsl:variable>					

						<Symbol>
							<xsl:choose>

								<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test ="COL22!=''">
									<xsl:value-of select ="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

							</Symbol>					

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>


						<xsl:variable name ="NetPosition">
							<xsl:choose>
								<xsl:when test="number(COL6)">
									<xsl:value-of select ="number(COL6)"/>
								</xsl:when>
								<xsl:when test="number(COL7)">
									<xsl:value-of select ="number(COL7)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>

						<Quantity>
							<xsl:choose>
								<xsl:when test ="number($NetPosition)">
									<xsl:value-of select ="$NetPosition"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<xsl:variable name ="varSide">
							<xsl:value-of select ="normalize-space(COL15)"/>
						</xsl:variable>



						<Side>
							<xsl:choose>
								<xsl:when test="string-length(COL22)=4">
									<xsl:choose>
										<xsl:when test="number(COL6)">
											<xsl:value-of select ="'Buy'"/>
										</xsl:when>
										<xsl:when test="number(COL7)">
											<xsl:value-of select ="'Sell'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="number(COL6)">
											<xsl:value-of select ="'Buy to Open'"/>
										</xsl:when>
										<xsl:when test="number(COL7)">
											<xsl:value-of select ="'Sell to Open'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>

						</Side>

						<xsl:variable name ="varCostBasis">
							<xsl:value-of select ="COL5"/>
						</xsl:variable>

						<AvgPX>
							<xsl:choose>

								<xsl:when test ="$varCostBasis &lt;0">
									<xsl:value-of select ="$varCostBasis*-1"/>
								</xsl:when>

								<xsl:when test ="$varCostBasis &gt;0">
									<xsl:value-of select ="$varCostBasis"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</AvgPX>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

						<!--<xsl:variable name ="varNotionalLocal">
							<xsl:value-of select ="number(COL13)"/>
						</xsl:variable>

						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test ="number($varNotionalLocal) ">
									<xsl:value-of select ="$varNotionalLocal"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>-->

						<CurrencySymbol>
							<xsl:value-of select="COL9"/>
						</CurrencySymbol>


						<Commission>
							<xsl:value-of select="200"/>
						</Commission>

						<CommissionBase>
							<xsl:value-of select="200"/>
						</CommissionBase>


						<SoftCommission>
							<xsl:value-of select="200"/>
						</SoftCommission>

						<SoftCommissionBase>
							<xsl:value-of select="200"/>
						</SoftCommissionBase>

						<Fees>
							<xsl:value-of select="200"/>
						</Fees>

						<FeesBase>
							<xsl:value-of select="200"/>
						</FeesBase>


						<ClearingBrokerFee>
							<xsl:value-of select="200"/>
						</ClearingBrokerFee>


						<ClearingBrokerFeeBase>
							<xsl:value-of select="200"/>
						</ClearingBrokerFeeBase>

						<MiscFees>
							<xsl:value-of select="200"/>
						</MiscFees>


						<MiscFeesBase>
							<xsl:value-of select="200"/>
						</MiscFeesBase>


						<StampDuty>
							<xsl:value-of select="200"/>
						</StampDuty>


						<StampDutyBase>
							<xsl:value-of select="200"/>
						</StampDutyBase>

						<ClearingFee>
							<xsl:value-of select="200"/>
						</ClearingFee>


						<ClearingFeeBase>
							<xsl:value-of select="200"/>
						</ClearingFeeBase>

						<SecFee>
							<xsl:value-of select="200"/>
						</SecFee>

						<SecFeeBase>
							<xsl:value-of select="200"/>
						</SecFeeBase>

						<OccFee>
							<xsl:value-of select="200"/>
						</OccFee>

						<OccFeeBase>
							<xsl:value-of select="200"/>
						</OccFeeBase>

						<OrfFee>
							<xsl:value-of select="200"/>
						</OrfFee>

						<OrfFeeBase>
							<xsl:value-of select="200"/>
						</OrfFeeBase>

					</Comparision>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>
	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
