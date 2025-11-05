<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="PB_NAME">
					<xsl:value-of select="'GS'"/>
				</xsl:variable>
				
				<xsl:variable name="PB_FUND_NAME">
					<xsl:value-of select="COL2"/>
				</xsl:variable>

				<xsl:variable name="PRANA_FUND_NAME">
					<xsl:value-of select="document('../ReconMappingXml/AccountMapping.Xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>
				<xsl:if test="number(COL9) and $PRANA_FUND_NAME != ''">
					<PositionMaster>



						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL22)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
						<xsl:variable name ="varBBCode">
							<xsl:value-of select ="normalize-space(substring(COL22, 1,2))"/>
						</xsl:variable>

						<xsl:variable name ="varBBKey">
							<xsl:value-of select ="normalize-space(substring(COL22, 6))"/>
						</xsl:variable>

						<xsl:variable name="PRANA_ROOT_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@UnderlyingCode"/>
						</xsl:variable>

						<xsl:variable name="PRANA_EXCHANGE_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@ExchangeName"/>
						</xsl:variable>

						<xsl:variable name="Root">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(COL22,' '))=4">
									<xsl:value-of select="concat($PRANA_ROOT_NAME, ' ', substring(COL22,3,2))"/>
								</xsl:when>
								
								<xsl:when test="string-length(substring-before(COL22,' '))=5">
									<xsl:value-of select="concat($PRANA_ROOT_NAME, ' ', substring(COL22,4,2), $PRANA_EXCHANGE_NAME)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="contains(COL22,'Index')='True' and string-length(substring-before(COL22,' '))=4" >
									<xsl:value-of select="concat(substring(COL22,1,2), ' ', substring(COL22,3,2))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
												
						</Symbol>	
						
						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME=''">
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<PositionStartDate>
							<xsl:value-of select="COL1"/>
						</PositionStartDate>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="number(COL9) &gt; 0">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="number(COL9) &lt; 0">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<CostBasis>
							<xsl:choose>
								<xsl:when test="number(COL19) &gt; 0">
									<xsl:value-of select="COL19"/>
								</xsl:when>
								<xsl:when test="number(COL19) &lt; 0">
									<xsl:value-of select="COL19*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<NetPosition>
							<xsl:choose>
								<xsl:when test="number(COL9) &gt; 0">
									<xsl:value-of select="COL9"/>
								</xsl:when>
								<xsl:when test="number(COL9) &lt; 0">
									<xsl:value-of select="COL9*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<CounterPartyID>
							<xsl:value-of select="23"/>
						</CounterPartyID>

						<xsl:variable name="PB_Currency" select="normalize-space(COL10)"/>
						<xsl:variable name="PRANA_Currency">
							<xsl:value-of select="document('../ReconMappingXml/CurrencyMapping.Xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@PBCurrencyName=$PB_Currency]/@PranaCurrencyID"/>
						</xsl:variable>

						<CurrencyID>
							<xsl:choose>
								<xsl:when test="number($PRANA_Currency)">
									<xsl:value-of select="$PRANA_Currency"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
							
						</CurrencyID>

						<TradeAttribute1>
							<xsl:choose>
								<xsl:when test="$PRANA_ROOT_NAME=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="concat($PRANA_ROOT_NAME, ' ', substring(COL22,3,2), $PRANA_EXCHANGE_NAME)"/>
								</xsl:otherwise>
							</xsl:choose>
						</TradeAttribute1>
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
