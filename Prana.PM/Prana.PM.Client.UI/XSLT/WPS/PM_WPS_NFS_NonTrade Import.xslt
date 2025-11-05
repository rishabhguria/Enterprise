<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

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

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Cash">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL14 div 100"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Cash) and (normalize-space(COL7)='DIV' or normalize-space(COL7)='CKP')">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PB_FUND_NAME" select="COL3"/>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

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


            <xsl:variable name="varDayName">
              <xsl:value-of select="substring(COL10,7,2)"/>
            </xsl:variable>

            <xsl:variable name="varMonthName">
              <xsl:value-of select="substring(COL10,5,2)"/>
            </xsl:variable>

            <xsl:variable name="varYearName">
              <xsl:value-of select="substring(COL10,1,4)"/>
            </xsl:variable>



            <xsl:variable name="varDateName">
              <xsl:value-of select="concat($varMonthName,'/',$varDayName,'/',$varYearName)"/>
            </xsl:variable>
            
            <Date>
							<xsl:value-of select="$varDateName"/>
						</Date>


						<xsl:variable name="AbsCash">
							<xsl:choose>
								<xsl:when test="$Cash &gt; 0">
									<xsl:value-of select="$Cash"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$Cash * -1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
            

            <xsl:variable name ="varTransactionSubTypeName" select="normalize-space(COL7)"/>
            <xsl:variable name ="varBatch" select="normalize-space(COL46)"/>
						<xsl:variable name="PRANA_ACRONYM_NAME">
							 <xsl:choose>
								<xsl:when test="contains($varTransactionSubTypeName,'CKP') and contains($varBatch,'CACT2')">
									<xsl:value-of select="'CashTransferOut'"/>
								</xsl:when>
                <xsl:when test="contains($varTransactionSubTypeName,'DIV') and contains($varBatch,'DAF02')">
                  <xsl:value-of select="'Dividend_Income'"/>
                </xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<JournalEntries>
							<xsl:choose>
								<xsl:when test="$Cash &lt; 0">
									<xsl:value-of select="concat($PRANA_ACRONYM_NAME, ':' , $AbsCash, '|Cash:',$AbsCash)"/>
								</xsl:when>
								<xsl:when test="$Cash &gt; 0">
									<xsl:value-of select="concat( 'Cash:',$AbsCash,'|', $PRANA_ACRONYM_NAME,':' , $AbsCash)"/>
								</xsl:when>
							</xsl:choose>
						</JournalEntries>

						<Description>
							<xsl:value-of select="COL19"/>
						</Description>

					</PositionMaster>
				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>