<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="varFundName" select ="normalize-space(COL3)"/>
				<xsl:if test ="(COL16 ='DV')and ($varFundName='10602437' or $varFundName='10602438' or $varFundName='10606110' or $varFundName='TOQFLPP' )">


					<PositionMaster>

						<!--FundNameSection -->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="$varFundName"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='DB']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="$PRANA_FUND_NAME !='' ">
								<FundName>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select="$varFundName"/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>


						<xsl:variable name="PRANA_FUND_ID">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='DB']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFundID"/>
						</xsl:variable>

						<FundID>
							<xsl:value-of select="$PRANA_FUND_ID"/>
						</FundID>

						<Dividend>
							<xsl:value-of select="COL42"/>
						</Dividend>

						<Symbol>
							<xsl:value-of select="COL22"/>
						</Symbol>



						<xsl:variable name ="varDate">
							<xsl:value-of select ="concat(substring(COL10,5,2),'/',substring(COL10,7,2),'/',substring(COL10,1,4))"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="$varDate !='' and $varDate != '*' and $varDate != '//'">
								<PayoutDate>
									<xsl:value-of select="$varDate"/>
								</PayoutDate>
							</xsl:when>
							<xsl:otherwise>
								<PayoutDate>
									<xsl:value-of select="''"/>
								</PayoutDate>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:choose>
							<xsl:when test ="$varDate !='' and $varDate != '*' and $varDate != '//'">
								<ExDate>
									<xsl:value-of select="$varDate"/>
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
