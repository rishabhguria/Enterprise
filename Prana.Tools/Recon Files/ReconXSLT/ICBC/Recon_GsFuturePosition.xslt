<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//Comparision">
				<xsl:if test="number(COL9)">
					<PositionMaster>

						<xsl:variable name="PB_FUND_NAME">
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.Xml')/FundMapping/PB[@Name='GS']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:variable name ="varBBCode">
							<xsl:value-of select ="normalize-space(substring(COL22, 1,2))"/>
						</xsl:variable>

						<xsl:variable name ="varBBKey">
							<xsl:value-of select ="normalize-space(substring(COL22, 6))"/>
						</xsl:variable>

						<xsl:variable name="PRANA_ROOT_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name='JPM']/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@UnderlyingCode"/>
						</xsl:variable>

						<xsl:variable name="PRANA_EXCHANGE_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name='JPM']/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@ExchangeName"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_ROOT_NAME=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="concat($PRANA_ROOT_NAME, ' ', substring(COL22,3,2), $PRANA_EXCHANGE_NAME)"/>
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

						<!--<PositionStartDate>
							<xsl:value-of select="COL1"/>
						</PositionStartDate>-->

						<Side>
							<xsl:choose>
								<xsl:when test="number(COL9) &gt; 0">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="number(COL9) &lt; 0">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<!--<CostBasis>
							<xsl:choose>
								<xsl:when test="number(COL20) &gt; 0">
									<xsl:value-of select="COL20"/>
								</xsl:when>
								<xsl:when test="number(COL20) &lt; 0">
									<xsl:value-of select="COL20*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>-->

						<Quantity>
							<xsl:choose>
								<xsl:when test="number(COL9)">
									<xsl:value-of select="COL9"/>
								</xsl:when>
								<!--<xsl:when test="number(COL9) &lt; 0">
									<xsl:value-of select="COL9*-1"/>
								</xsl:when>-->
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>


						<PBSymbol>
							<xsl:value-of select="COL22"/>
						</PBSymbol>

						<xsl:variable name="varMarkPrice">
							<xsl:value-of select="COL20"/>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test="number($varMarkPrice) ">
									<xsl:value-of select="$varMarkPrice"/>
								</xsl:when>
								<!--<xsl:when test="number($varMarkPrice) &lt; 0">
									<xsl:value-of select="$varMarkPrice*-1"/>
								</xsl:when>-->
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>


						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
