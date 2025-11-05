<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>
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
	<xsl:template match="/">

		<DocumentElement>
			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Cashlocal">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL9"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test ="number($Cashlocal)">

					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Velocity'"/>
						</xsl:variable>

					 <xsl:variable name="PB_FUND_NAME">
           <xsl:choose>
				<xsl:when test="COL2='*' or COL2=''">
           <xsl:value-of select="concat(normalize-space(COL3),'-',normalize-space(COL4),'-',normalize-space(COL5),'-',normalize-space(COL6))"/>
           </xsl:when>
           <xsl:when test="COL3='*' or COL3=''">
				<xsl:value-of select="concat(normalize-space(COL2),'-',normalize-space(COL4),'-',normalize-space(COL5),'-',normalize-space(COL6))"/>
           </xsl:when>
		   <xsl:when test="COL4='*' or COL4=''">
				<xsl:value-of select="concat(normalize-space(COL2),'-',normalize-space(COL3),'-',normalize-space(COL5),'-',normalize-space(COL6))"/>
           </xsl:when>
		   <xsl:when test="COL5='*' or COL5=''">
				<xsl:value-of select="concat(normalize-space(COL2),'-',normalize-space(COL3),'-',normalize-space(COL4),'-',normalize-space(COL6))"/>
           </xsl:when>
		     <xsl:when test="COL6='*' or COL6=''">
				<xsl:value-of select="concat(normalize-space(COL2),'-',normalize-space(COL3),'-',normalize-space(COL4),'-',normalize-space(COL5))"/>
           </xsl:when>
           <xsl:otherwise>
				<xsl:value-of select="concat(normalize-space(COL2),'-',normalize-space(COL3),'-',normalize-space(COL4),'-',normalize-space(COL5),'-',normalize-space(COL6))"/>
           </xsl:otherwise>
           </xsl:choose>
        </xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select='$PB_FUND_NAME'/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>
						
						<Symbol>
							<xsl:value-of select="normalize-space(COL8)"/>
						</Symbol>


						<!-- <xsl:variable name ="varCurrency"> -->
							<!-- <xsl:value-of select ="normalize-space(COL8)"/> -->
						<!-- </xsl:variable> -->

						<!-- <Currency> -->
							<!-- <xsl:value-of select="$varCurrency"/> -->
						<!-- </Currency>						 -->

										

						<CashValueLocal>						  
							  <xsl:choose>
								<xsl:when test ="$Cashlocal &lt;0">
									<xsl:value-of select ="$Cashlocal*-1"/>
								</xsl:when>
								<xsl:when test ="$Cashlocal &gt;0">
									<xsl:value-of select ="$Cashlocal"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>							
						</CashValueLocal>
						
						<xsl:variable name="varBaseCash">
                           <xsl:call-template name="Translate">
                           <xsl:with-param name="Number" select="COL9"/>
                           </xsl:call-template>
                         </xsl:variable>
						 
                            <CashValueBase>
                              <xsl:choose>
								<xsl:when test ="$varBaseCash &lt;0">
									<xsl:value-of select ="$varBaseCash*-1"/>
								</xsl:when>
								<xsl:when test ="$varBaseCash &gt;0">
									<xsl:value-of select ="$varBaseCash"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
                            </CashValueBase>

						<!-- <Symbol> -->
							<!-- <xsl:value-of select="'USD'"/> -->
						<!-- </Symbol> -->
						
						<xsl:variable name="TradeDate" select="COL1"/>
						<TradeDate>
						<xsl:value-of select="$TradeDate"/>
						</TradeDate>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>