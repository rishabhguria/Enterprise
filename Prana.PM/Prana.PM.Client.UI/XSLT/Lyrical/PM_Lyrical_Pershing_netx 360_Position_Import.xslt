<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>


	<xsl:template match="/">
		
		<DocumentElement>
			
			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position" select="COL6"/>				

				<xsl:if test="number($Position) and COL4='EQUITY'">
					
					<PositionMaster>
						
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Pershing'"/>
						</xsl:variable>
						
						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL5)"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
						<xsl:variable name="Symbol" select="COL2"/>

						<xsl:variable name="PB_FUND_NAME" select="substring(COL1,1,9)"/>
						
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								
								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
						
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
						
						<NetPosition>
							<xsl:choose>
								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$Position"/>
								</xsl:when>
								
								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$Position * (-1)"/>
								</xsl:when>
							</xsl:choose>
						</NetPosition>
						
						<SideTagValue>
							<xsl:choose>

								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="'1'"/>
								</xsl:when>


								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>

							</xsl:choose>
						</SideTagValue>						
						
						<CostBasis>
							<xsl:value-of select="0"/>
						</CostBasis>
						
						<PositionStartDate>
							<xsl:value-of select="''"/>
						</PositionStartDate>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
