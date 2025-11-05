<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0">
	<xsl:output method="xml" indent="yes" />

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


	<!-- Define keys to group by Symbol,  and AccountName -->
	<!--Symbol=COL6,AccountName=COL1-->
	
	<!--<xsl:key name="grouped" match="PositionMaster" use="concat(COL2, '|', COL13, '|', COL1, '|', COL3, '|', COL14)"/>-->
	
	<xsl:key name="grouped" match="PositionMaster" use="concat(COL6, '|', COL1)"/>

	<xsl:template match="/">
		<DocumentElement xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">
			<!-- Loop over each unique group of PositionMaster -->
			<xsl:for-each select="//PositionMaster[generate-id() = generate-id(key('grouped', concat(COL6, '|', COL1))[1])]">
				<xsl:variable name="symbol" select="COL6"/>
				<xsl:variable name="accountname" select="COL1"/>
				

				<xsl:variable name="groupedNetPosition" select="sum(key('grouped', concat($symbol, '|', $accountname))/COL13)"/>
				<!--<xsl:variable name="groupedCOmmission" select="sum(key('grouped', concat($symbol, '|', $accountname))/COL4)"/>-->
			
				<!-- Get the nodes for this group -->
				<xsl:variable name="groupedNodes" select="key('grouped', concat($symbol, '|', $accountname))"/>

                <!-- Calculate the weighted average price -->               

			<xsl:variable name="tempResults">
            <sum>
                <xsl:for-each select="key('grouped', concat($symbol, '|', $accountname))">
                    <value>
                        <xsl:value-of select="number(COL13) * number(COL15)"/>
                    </value>
                </xsl:for-each>
            </sum>
        </xsl:variable>

         <!--Convert variable into node-set and sum the values--> 
        <xsl:variable name="sumWeightedPrice"
            select="sum(msxsl:node-set($tempResults)/sum/value)"/>	

				
				<!-- Avoiding division by zero -->
        <xsl:variable name="weightedAvgPrice">
            <xsl:choose>
                <xsl:when test="$groupedNetPosition != 0">
                    <xsl:value-of select="$sumWeightedPrice div $groupedNetPosition"/>
                </xsl:when>
                <xsl:otherwise>0</xsl:otherwise>
            </xsl:choose>
        </xsl:variable>
				
				<xsl:variable name="varNetPosition">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="$groupedNetPosition" />
					</xsl:call-template>
				</xsl:variable>


				<xsl:if test="number($varNetPosition)">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''" />
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="''" />
						</xsl:variable>

						<xsl:variable name="varSymbol">
							<xsl:value-of select="normalize-space($symbol)" />
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol" />
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME" />
								</xsl:when>

								<xsl:when test="$varSymbol !=''">
									<xsl:value-of select="$varSymbol" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME" />
								</xsl:otherwise>
							</xsl:choose>

						</Symbol>

						<xsl:variable name="PB_FUND_NAME" select="$accountname" />

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund" />
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME" />
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME" />
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$varNetPosition &gt; 0 ">
									<xsl:value-of select="'1'"/>
								</xsl:when>

								<xsl:when test="$varNetPosition &lt; 0">
									<xsl:value-of select="'5'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>


						<NetPosition>
							<xsl:choose>
								<xsl:when test="$varNetPosition &gt; 0">
									<xsl:value-of select="$varNetPosition" />
								</xsl:when>
								<xsl:when test="$varNetPosition &lt; 0">
									<xsl:value-of select="$varNetPosition * (-1)" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0" />
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="varCostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="$weightedAvgPrice"/>
							</xsl:call-template>
						</xsl:variable>
						<CostBasis>
							<xsl:choose>
								<xsl:when test="$varCostBasis &gt; 0">
									<xsl:value-of select="$varCostBasis" />
								</xsl:when>
								<xsl:when test="$varCostBasis &lt; 0">
									<xsl:value-of select="$varCostBasis * (-1)" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0" />
								</xsl:otherwise>
							</xsl:choose>
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