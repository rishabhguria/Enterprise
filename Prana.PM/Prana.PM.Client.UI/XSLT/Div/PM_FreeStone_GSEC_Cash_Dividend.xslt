<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
        <xsl:if test="COL2 = 'QMI' or COL2 = 'DIV'">
					<!--TABLE-->
					<PositionMaster>

						<!--FundNameSection -->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL1,'&quot;','')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GSEC']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>


						<FundName>
							<xsl:value-of select='$PRANA_FUND_NAME'/>
						</FundName>

						<xsl:variable name="PRANA_FUND_ID">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GSEC']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFundID"/>
						</xsl:variable>

						<FundID>
							<xsl:value-of select="$PRANA_FUND_ID"/>
						</FundID>

						<Symbol>
							<xsl:value-of select="COL3"/>
						</Symbol>
						<Dividend>
							<xsl:value-of select="translate(COL12,'N/A','0')"/>
						</Dividend>
						<PayoutDate>
							<xsl:value-of select="COL4"/>
						</PayoutDate>
						<ExDate>
							<xsl:value-of select="COL4"/>
						</ExDate>

					</PositionMaster>
					
				</xsl:if >
			</xsl:for-each>			
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
