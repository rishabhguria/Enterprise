<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">

				<xsl:if test ="substring(COL1,123,3)='DIV' or substring(COL1,123,3)='DVD'"> 
				
					<PositionMaster>

						<!--FundNameSection -->
						<xsl:variable name="varFundName" select ="normalize-space(substring(COL1,4,5))"/>
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="$varFundName"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='ATRADING']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<FundName>
							<xsl:value-of select='$PRANA_FUND_NAME'/>
						</FundName>

						<xsl:variable name="PRANA_FUND_ID">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='ATRADING']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFundID"/>
						</xsl:variable>

						<FundID>
							<xsl:value-of select="$PRANA_FUND_ID"/>
						</FundID>

						<xsl:variable name ="varDividend">
							<xsl:value-of select ="substring(COL1,86,17)"/>
						</xsl:variable>

						<xsl:variable name ="varDividendInt">
							<xsl:value-of select ="number(substring($varDividend,1,9))"/>
						</xsl:variable>

						<xsl:variable name ="varDividendFrac">
							<xsl:value-of select ="number(substring($varDividend,10,7))"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="boolean(number($varDividend))">
								<Dividend>
									<xsl:value-of select="number(concat($varDividendInt,'.',$varDividendFrac))"/>
								</Dividend>
							</xsl:when>
							<xsl:otherwise>
								<Dividend>
									<xsl:value-of select="0"/>
								</Dividend>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="normalize-space(substring(COL1,72,8))!=''">
								<PayoutDate>
									<xsl:value-of select="concat(substring(COL1,72,2),'/',substring(COL1,74,2),'/','20',substring(COL1,78,2))"/>
								</PayoutDate>
							</xsl:when>
							<xsl:otherwise>
								<PayoutDate>
									<xsl:value-of select="''"/>
								</PayoutDate>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="normalize-space(substring(COL1,72,8))!=''">
								<ExDate>
									<xsl:value-of select="concat(substring(COL1,72,2),'/',substring(COL1,74,2),'/','20',substring(COL1,78,2))"/>
								</ExDate>
							</xsl:when>
							<xsl:otherwise>
								<ExDate>
									<xsl:value-of select="''"/>
								</ExDate>
							</xsl:otherwise>
						</xsl:choose>
				
					</PositionMaster>
					
				</xsl:if >
			</xsl:for-each>			
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
