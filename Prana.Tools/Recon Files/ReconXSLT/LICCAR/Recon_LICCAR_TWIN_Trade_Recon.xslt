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

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
			<xsl:choose>
				<xsl:when test="$Month='Jan'">
					<xsl:value-of select="'01'"/>
				</xsl:when>
				<xsl:when test="$Month='Feb' ">
					<xsl:value-of select="'02'"/>
				</xsl:when>
				<xsl:when test="$Month='Mar' ">
					<xsl:value-of select="'03'"/>
				</xsl:when>
				<xsl:when test="$Month='Apr' ">
					<xsl:value-of select="'04'"/>
				</xsl:when>
				<xsl:when test="$Month='May' ">
					<xsl:value-of select="'05'"/>
				</xsl:when>
				<xsl:when test="$Month='Jun' ">
					<xsl:value-of select="'06'"/>
				</xsl:when>
				<xsl:when test="$Month='Jul'">
					<xsl:value-of select="'07'"/>
				</xsl:when>
				<xsl:when test="$Month='Aug'  ">
					<xsl:value-of select="'08'"/>
				</xsl:when>
				<xsl:when test="$Month='Sep' ">
					<xsl:value-of select="'09'"/>
				</xsl:when>
				<xsl:when test="$Month='Oct' ">
					<xsl:value-of select="'10'"/>
				</xsl:when>
				<xsl:when test="$Month='Nov' ">
					<xsl:value-of select="'11'"/>
				</xsl:when>
				<xsl:when test="$Month='Dec' ">
					<xsl:value-of select="'12'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL15)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($varQuantity) and (normalize-space(COL8)='Buy' or normalize-space(COL8)='CoverShort' or normalize-space(COL8)='Sell' or normalize-space(COL8)='SellShort' )">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Fidelity'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL13)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="Symbol">
							<xsl:value-of select="normalize-space(COL9)"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL3)"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund" />
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME !=''">
									<xsl:value-of select="$PRANA_FUND_NAME" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME" />
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<Quantity>
							<xsl:choose>
								<xsl:when test="number($varQuantity)">
									<xsl:value-of select="$varQuantity"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<xsl:variable name="AvgPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL16)"/>
							</xsl:call-template>
						</xsl:variable>

						<AvgPX>
							<xsl:choose>
								<xsl:when test="$AvgPrice &gt; 0">
									<xsl:value-of select="$AvgPrice"/>
								</xsl:when>
								<xsl:when test="$AvgPrice &lt; 0">
									<xsl:value-of select="$AvgPrice * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>
						
						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL20)"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test="$Commission &gt; 0">
									<xsl:value-of select="$Commission"/>
								</xsl:when>
								<xsl:when test="$Commission &lt; 0">
									<xsl:value-of select="$Commission * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="NetNotionalValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL24)"/>
							</xsl:call-template>
						</xsl:variable>

						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test="$NetNotionalValue &gt; 0">
									<xsl:value-of select="$NetNotionalValue"/>
								</xsl:when>
								<xsl:when test="$NetNotionalValue &lt; 0">
									<xsl:value-of select="$NetNotionalValue * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>

						<xsl:variable name="varNetNotionalValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL25)"/>
							</xsl:call-template>
						</xsl:variable>

						<NetNotionalValueBase>
							<xsl:choose>
								<xsl:when test="$varNetNotionalValueBase &gt; 0">
									<xsl:value-of select="$varNetNotionalValueBase"/>
								</xsl:when>
								<xsl:when test="$varNetNotionalValueBase &lt; 0">
									<xsl:value-of select="$varNetNotionalValueBase * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValueBase>

						<Side>
							<xsl:choose>
								<xsl:when test="normalize-space(COL8)='Buy'">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL8)='Sell'">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL8)='CoverShort'">
									<xsl:value-of select="'Buy to Close'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>
												
						<xsl:variable name="varSDate" select="substring-before(normalize-space(COL7),' ')"/>

						<xsl:variable name="varSMonth">
							<xsl:call-template name="MonthCode">
								<xsl:with-param name="Month" select="substring-before(substring-after(normalize-space(COL7),' '),' ')"/>
							</xsl:call-template>
						</xsl:variable>
						
						<xsl:variable name="varSYear" select="substring(substring-after(substring-after(normalize-space(COL7),' '),' '),3,2)"/>

						<SettlementDate>
							<xsl:value-of select="concat($varSMonth,'/',$varSDate,'/',$varSYear)"/>
						</SettlementDate>
						
						<xsl:variable name="varTDate" select="substring-before(normalize-space(COL6),' ')"/>

						<xsl:variable name="varTMonth">
							<xsl:call-template name="MonthCode">
								<xsl:with-param name="Month" select="substring-before(substring-after(normalize-space(COL6),' '),' ')"/>
							</xsl:call-template>
						</xsl:variable>
						
						<xsl:variable name="varTYear" select="substring(substring-after(substring-after(normalize-space(COL6),' '),' '),3,2)"/>
						
						<TradeDate>
							<xsl:value-of select="concat($varTMonth,'/',$varTDate,'/',$varTYear)"/>
						</TradeDate>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>