<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select="//Comparision">
				<xsl:if test="number(COL10)">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Morgan Stanley'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL2"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="COL1!='' or COL1!=' '">
									<xsl:value-of select="COL1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="'Richard S. Linhart: 476-080678-293'"/>
						</xsl:variable>


						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME=''">
									<xsl:value-of select='$PB_FUND_NAME'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>					


						<xsl:variable name="varAvgPrice">
							<xsl:value-of select="COL5"/>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test ='number($varAvgPrice) &lt; 0'>
									<xsl:value-of select ="$varAvgPrice*-1"/>
								</xsl:when>
								<xsl:when test ='number($varAvgPrice) &gt; 0'>
									<xsl:value-of select ='$varAvgPrice'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>



						<xsl:variable name="varQuantity">
							<xsl:value-of select="COL10"/>
						</xsl:variable>

						<Quantity>
							<xsl:choose>
								<xsl:when test ='number($varQuantity) &lt; 0'>
									<xsl:value-of select ='$varQuantity*-1'/>
								</xsl:when>
								<xsl:when test ='number($varQuantity) &gt; 0'>
									<xsl:value-of select ='$varQuantity'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<!--<xsl:variable name="varSide">
							<xsl:value-of select="COL11"/>
						</xsl:variable>-->

						<Side>
							<xsl:choose>
								<xsl:when test ='number($varQuantity) &lt; 0'>
									<xsl:value-of select ="'Sell short'"/>
								</xsl:when>
								<xsl:when test ='number($varQuantity) &gt; 0'>
									<xsl:value-of select ="'Buy'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>


						<xsl:variable name="MarketValue">
							<xsl:value-of select="COL11"/>
						</xsl:variable>

						<MarketValue>
							<xsl:choose>
								<xsl:when test ='number($MarketValue) &lt; 0'>
									<xsl:value-of select ="$MarketValue*-1"/>
								</xsl:when>
								<xsl:when test ='number($MarketValue) &gt; 0'>
									<xsl:value-of select ='$MarketValue'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValue>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
