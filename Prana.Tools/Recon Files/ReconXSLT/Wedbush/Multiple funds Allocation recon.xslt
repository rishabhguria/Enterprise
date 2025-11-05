<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select="//Comparision">

				<xsl:if test="number(COL3)">

					<PositionMaster>

						<xsl:variable name="varPrice">
							<xsl:value-of select="COL4"/>
						</xsl:variable>

							<xsl:variable name="varPosition">
							<xsl:value-of select="COL3"/>
						</xsl:variable>
						
						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL6"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='GS']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

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

						<Symbol>
							<xsl:value-of select="COL1"/>
						</Symbol>

						<Quantity>
							<xsl:choose>
								<xsl:when test="number($varPosition) ">
									<xsl:value-of select="$varPosition"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<Side>
              <xsl:value-of select="COL7"/>
						</Side>

						<AvgPX>
							<xsl:choose>
								<xsl:when test="number($varPrice) &gt; 0">
									<xsl:value-of select="$varPrice"/>
								</xsl:when>
								<xsl:when test="number($varPrice) &lt; 0">
									<xsl:value-of select="$varPrice*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>

            <Asset>
              <xsl:value-of select="COL8"/>
            </Asset>

            <!--<NetNotionalValue>
              <xsl:choose>
                <xsl:when test="number(COL13) &gt; 0">
                  <xsl:value-of select="COL13"/>
                </xsl:when>
                <xsl:when test="number(COL13) &lt; 0">
                  <xsl:value-of select="COL13*-1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>

						<Commission>
							<xsl:choose>
								<xsl:when test="number(COL10) &gt; 0">
									<xsl:value-of select="COL10"/>
								</xsl:when>
								<xsl:when test="number(COL10) &lt; 0">
									<xsl:value-of select="COL10*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<Fees>
							<xsl:value-of select="COL11+COL12"/>
						</Fees>-->
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
   
</xsl:template>

</xsl:stylesheet> 
