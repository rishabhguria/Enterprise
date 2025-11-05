<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	


	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//Comparision">
				<xsl:if test="normalize-space(COL4)='S' or normalize-space(COL4)='L'"/>
				<PositionMaster>
					
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="COL18"/>
					</xsl:variable>

					<xsl:variable name = "PB_CURRENCY_NAME" >
						<xsl:value-of select="COL12"/>
					</xsl:variable>

					<xsl:variable name = "PB_ASSET_NAME" >
						<xsl:value-of select="COL16"/>
					</xsl:variable>
					
					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:choose>
							<xsl:when test ="COL18 = 'DBAB' and normalize-space(COL16) = 'Equities - Swap'">
								<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode=$PB_FUND_NAME and @Currency = $PB_CURRENCY_NAME and @Asset = $PB_ASSET_NAME]/@PranaFund"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode = $PB_FUND_NAME and @Asset = $PB_ASSET_NAME]/@PranaFund"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</xsl:variable>

					<AccountName>
						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<xsl:value-of select='$PB_FUND_NAME'/>
						</xsl:when>
						<xsl:otherwise>
								<xsl:value-of select='$PRANA_FUND_NAME'/>
						</xsl:otherwise>
					</xsl:choose >
					</AccountName>


					<Symbol>
						<xsl:value-of select="''"/>
					</Symbol>
					
					<SEDOL>
						<xsl:value-of select="COL14"/>
					</SEDOL>

					<MarkPrice>
						<xsl:choose>
							<xsl:when test="number(COL9)">
								<xsl:value-of select="COL9"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</MarkPrice>

					<!--<Quantity>
						<xsl:choose>
							<xsl:when test="COL5 &gt; 0">
								<xsl:value-of select="COL5"/>
							</xsl:when>
							<xsl:when test="COL5 &lt; 0">
								<xsl:value-of select="COL5*(-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Quantity>-->
					<Quantity>
						<xsl:choose>
							<xsl:when test="number(COL5)">
								<xsl:value-of select="COL5"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Quantity>
					
					<Side>
						<xsl:choose>
							<xsl:when test="normalize-space(COL4)='L'">
								<xsl:value-of select="'Buy'"/>
							</xsl:when>
							<xsl:when test="normalize-space(COL4)='S'">
								<xsl:value-of select="'Sell'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>

						</xsl:choose>
					</Side>

					<SMRequest>
						<xsl:value-of select="'TRUE'"/>
					</SMRequest>

				</PositionMaster>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


