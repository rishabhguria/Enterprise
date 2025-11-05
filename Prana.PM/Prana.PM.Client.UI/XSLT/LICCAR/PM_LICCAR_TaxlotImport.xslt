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
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL26"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test ="number($varQuantity)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varSymbol">
							<xsl:value-of select="normalize-space(COL7)"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL8)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varSymbol!='' or $varSymbol!='*'">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL2)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<NetPosition>
							<xsl:choose>
								<xsl:when test="$varQuantity &gt; 0">
									<xsl:value-of select="$varQuantity"/>
								</xsl:when>
								<xsl:when test="$varQuantity &lt; 0">
									<xsl:value-of select="$varQuantity * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>
						

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$varQuantity &lt;0">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="$varQuantity &gt;0">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<xsl:variable name = "varCostBasisMoney" >
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL31"/>
							</xsl:call-template>
						</xsl:variable>						

						<xsl:variable name="varCostBasis">
							<xsl:choose>
								<xsl:when test ="$varQuantity !=0">
									<xsl:value-of select ="$varCostBasisMoney div $varQuantity"/>
								</xsl:when>								
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>
								<xsl:when test ="$varCostBasis &lt;0 ">
									<xsl:value-of select ="$varCostBasis*(-1)"/>
								</xsl:when>
								<xsl:when test ="$varCostBasis &gt;0">
									<xsl:value-of select ="$varCostBasis"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<xsl:variable name="varFXRate">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL5"/>
							</xsl:call-template>
						</xsl:variable>					

						<FXRate>
							<xsl:choose>
								<xsl:when test ="$varFXRate &lt;0">
									<xsl:value-of select ="$varFXRate*(-1)"/>
								</xsl:when>
								<xsl:when test ="$varFXRate &gt;0">
									<xsl:value-of select ="$varFXRate"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</FXRate>

						<xsl:variable name = "varPositionDay" >
							<xsl:value-of select="substring(COL43,7,2)"/>
						</xsl:variable>
						<xsl:variable name = "varPositionMonth" >
							<xsl:value-of select="substring(COL43,5,2)"/>
						</xsl:variable>
						<xsl:variable name = "varPositionYear" >
							<xsl:value-of select="substring(COL43,1,4)"/>
						</xsl:variable>

						<xsl:variable name = "varPositionDate" >
							<xsl:value-of select="concat($varPositionMonth,'/',$varPositionDay,'/',$varPositionYear)"/>
						</xsl:variable>

						<PositionStartDate>
							<xsl:value-of select="$varPositionDate"/>
						</PositionStartDate>

						<xsl:variable name="varOriginalPurchaseDate">
							<xsl:value-of select="substring(COL36,7,2)"/>
						</xsl:variable>

						<xsl:variable name="varOriginalPurchaseMonth">
							<xsl:value-of select="substring(COL36,5,2)"/>
						</xsl:variable>

						<xsl:variable name="varOriginalPurchaseYear">
							<xsl:value-of select="substring(COL36,1,4)"/>
						</xsl:variable>

						<xsl:variable name = "varOriginalPurchaseDateFull" >
							<xsl:value-of select="concat($varOriginalPurchaseMonth,'/',$varOriginalPurchaseDate,'/',$varOriginalPurchaseYear)"/>
						</xsl:variable>

						<OriginalPurchaseDate>
							<xsl:value-of select="$varOriginalPurchaseDateFull"/>
						</OriginalPurchaseDate>

						<xsl:variable name = "varPositionSettleDay" >
							<xsl:value-of select="substring(COL44,7,2)"/>
						</xsl:variable>
						<xsl:variable name = "varPositionSettleMonth" >
							<xsl:value-of select="substring(COL44,5,2)"/>
						</xsl:variable>
						<xsl:variable name = "varPositionSettleYear" >
							<xsl:value-of select="substring(COL44,1,4)"/>
						</xsl:variable>

						<xsl:variable name = "varPositionSettleFullDate" >
							<xsl:value-of select="concat($varPositionSettleMonth,'/',$varPositionSettleDay,'/',$varPositionSettleYear)"/>
						</xsl:variable>
				
						<PositionSettlementDate>
							<xsl:value-of select="$varPositionSettleFullDate"/>
						</PositionSettlementDate>

						<xsl:variable name="varCurrency">
							<xsl:value-of select="normalize-space(COL4)"/>
						</xsl:variable>											

						<SettlCurrencyName>
							<xsl:value-of select="$varCurrency"/>
						</SettlCurrencyName>
						
						<FXConversionMethodOperator>
							<xsl:choose>
								<xsl:when test ="$varCurrency='CAD'">
									<xsl:value-of select ="'D'"/>
								</xsl:when>							
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</FXConversionMethodOperator>
						
					    <xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="normalize-space(COL42)"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_BROKER_ID">
							<xsl:value-of select="document('../ReconMappingXml/CounterPartyMapping.xml')/CounterPartyMapping/PB[@Name=$PB_NAME]/CounterPartyData[@MappedBrokerCode=$PB_BROKER_NAME]/@BrokerCode"/>
						</xsl:variable>
						
						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="number($PRANA_BROKER_ID!='')">
									<xsl:value-of select="$PRANA_BROKER_ID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>


					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>