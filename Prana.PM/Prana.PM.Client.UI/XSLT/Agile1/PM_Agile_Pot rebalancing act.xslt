<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test ="number(COL7)">
					<PositionMaster>

						<xsl:variable name="varCostBasis">
							<xsl:value-of select="COL8"/>
						</xsl:variable>

						<xsl:variable name="varNetPosition">
							<xsl:value-of select="COL7"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='GS']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select='$PB_FUND_NAME'/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>


						<PositionStartDate>
							<xsl:value-of select="COL2"/>
						</PositionStartDate>

						<Symbol>
							<xsl:value-of select="COL5"/>
						</Symbol>


						<xsl:choose>
							<xsl:when test="$varNetPosition &lt; 0">
								<NetPosition>
									<xsl:value-of select="$varNetPosition * (-1)"/>
								</NetPosition>
							</xsl:when>
							<xsl:when test="$varNetPosition &gt; 0">
								<NetPosition>
									<xsl:value-of select="$varNetPosition"/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>



						<SideTagValue>
							<xsl:choose>
								<xsl:when test="COL4 = 'BUY' ">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="COL4 = 'STO' ">
									<xsl:value-of select="'C'"/>
								</xsl:when>
								<xsl:when test="COL4 = 'BTC' ">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="COL4 = 'SEL' ">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="COL4 = 'SSL' ">
									<xsl:value-of select="'5'"/>
								</xsl:when>
							
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<CostBasis>
							<xsl:choose>
								<xsl:when test ="number($varCostBasis) &gt; 0">
									<xsl:value-of select="$varCostBasis"/>
								</xsl:when>
								<xsl:when test ="number($varCostBasis) &lt; 0">
									<xsl:value-of select="$varCostBasis*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
